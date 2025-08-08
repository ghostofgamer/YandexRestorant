using System;
using System.IO;
using ADSContent;
using CoppraGames;
using DailyTimerContent;
using EnergyContent;
using I2.Loc;
using PlayerContent.LevelContent;
using SettingsContent.SoundContent;
using SoContent;
using TMPro;
using UI.Buttons;
using UI.Screens;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace FortuneContent
{
    public class Fortune : MonoBehaviour
    {
        [SerializeField] private TMP_Text _spinValueText;
        [SerializeField] private SpinWheelController _spinWheelController;
        [SerializeField] private DailyTimerFortune _dailyTimerFortune;
        [SerializeField] private DailyTimerFortune _dailyTimerADSFortune;
        [SerializeField] private GameObject[] _spinButtons;
        [SerializeField] private GameObject _spinFreeButton;
        [SerializeField] private GameObject _spinValueButton;
        [SerializeField] private GameObject _spinADSButton;
        [SerializeField] private GameObject _backWinText;
        [SerializeField] private TMP_Text _prizeText;
        [SerializeField] private ADS _ads;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Animator _fortuneButton;
        [SerializeField] private FortuneScreen _fortuneScreen;
        [SerializeField] private FortuneSpriteConfig _fortuneSpriteConfig;
        [SerializeField] private SoundPlayer _soundPlayer;
        [SerializeField] private Button _openFortuneButton;
        [SerializeField] private GameObject _touchTaskFreeSpin;
        [SerializeField] private Energy _energy;

        private int _currentValueSpin = 1;
        private string filePath;
        private bool _isFreeButtonUsed = false;

        private Prize[] prizeMap = new Prize[]
        {
            new Prize { Type = PrizesFortune.Money, Value = 10 },
            new Prize { Type = PrizesFortune.Spin, Value = 3 },
            new Prize { Type = PrizesFortune.Money, Value = 50 },
            new Prize { Type = PrizesFortune.XP, Value = 20 },
            new Prize { Type = PrizesFortune.Energy, Value = 10 },
            new Prize { Type = PrizesFortune.XP, Value = 600 },
            new Prize { Type = PrizesFortune.Money, Value = 1000 },
            new Prize { Type = PrizesFortune.XP, Value = 75 },
            new Prize { Type = PrizesFortune.Energy, Value = 3 },
            new Prize { Type = PrizesFortune.XP, Value = 1500 },
        };

        public event Action FreeSpinDayCompleted;

        public event Action FreeSpinUsed;

        private void OnEnable()
        {
            _spinWheelController.PrizeCompleted += SetPrize;
            // _dailyTimerFortune.TimeOverCompleted += ActivateFreeSpinButton;
            _dailyTimerFortune.TimeNotOverCompleted += ActiveOtherSpinButton;
            _fortuneScreen.FortuneScreenClosed += AnimateButton;
            _playerLevel.LevelChanged += ActivateOpenFortuneButton;
        }

        private void OnDisable()
        {
            _spinWheelController.PrizeCompleted -= SetPrize;
            // _dailyTimerFortune.TimeOverCompleted -= ActivateFreeSpinButton;
            _dailyTimerFortune.TimeNotOverCompleted -= ActiveOtherSpinButton;
            _fortuneScreen.FortuneScreenClosed -= AnimateButton;
            _playerLevel.LevelChanged -= ActivateOpenFortuneButton;
        }

        private void Start()
        {
            ActivateOpenFortuneButton(_playerLevel.CurrentLevel);
            
            _isFreeButtonUsed = PlayerPrefs.GetInt("FreeSpinUsed", 0) > 0;
            
            _touchTaskFreeSpin.SetActive(!_isFreeButtonUsed);
            _spinFreeButton.gameObject.SetActive(!_isFreeButtonUsed);
            
            Debug.Log("_isFreeButtonUsed " + _isFreeButtonUsed);

            _dailyTimerFortune.UpdateInfo();
            filePath = Path.Combine(Application.persistentDataPath, "spinData.json");
            LoadSpinData();
            _spinValueText.text=$"{LocalizationManager.GetTermTranslation("BALANCE")}: {_currentValueSpin.ToString()}";
            AnimateButton();
        }

        public void AddSpins(int value)
        {
            _currentValueSpin += value;
            _dailyTimerFortune.UpdateInfo();
            // _spinValueText.text = $"BALANCE: {_currentValueSpin.ToString()}";
            _spinValueText.text=$"{LocalizationManager.GetTermTranslation("BALANCE")}: {_currentValueSpin.ToString()}";
            _spinValueButton.SetActive(_currentValueSpin > 0);
            SaveSpinData();
        }

        public void OnShow()
        {
            if (!_isFreeButtonUsed)
            {
                // AppMetrica.ReportEvent("TaskFortuna");
                FreeSpinUsed?.Invoke();
                PlayerPrefs.SetInt("FreeSpinUsed", 1);
                _isFreeButtonUsed = true;
            }
            
            _fortuneScreen.OpenScreen();
            _spinValueButton.SetActive(_currentValueSpin > 0);
            _spinValueText.text=$"{LocalizationManager.GetTermTranslation("BALANCE")}: {_currentValueSpin.ToString()}";
        }

        public void SpinWheel()
        {
            if (_currentValueSpin <= 0)
                return;

            if (_spinWheelController.IsStarted)
                return;
            
            _touchTaskFreeSpin.SetActive(false);
            SoundPlayer.Instance.PlayButtonClick();
            _currentValueSpin--;
            SaveSpinData();
            Spin();
            _spinValueText.text=$"{LocalizationManager.GetTermTranslation("BALANCE")}: {_currentValueSpin.ToString()}";
        }

        public void SpinFree()
        {
            SpinWheel();
            
            /*if (!_isFreeButtonUsed)
            {
                AppMetrica.ReportEvent("TaskFortuna");
                FreeSpinUsed?.Invoke();
                PlayerPrefs.SetInt("FreeSpinUsed", 1);
            }*/
            
            _touchTaskFreeSpin.SetActive(false);
            _spinFreeButton.gameObject.SetActive(false);
            _spinValueButton.SetActive(_currentValueSpin > 0);
        }

        public void SpinADS()
        {
            if (_spinWheelController.IsStarted)
                return;
            
            /*if (!_isFreeButtonUsed)
            {
                AppMetrica.ReportEvent("TaskFortuna");
                FreeSpinUsed?.Invoke();
                PlayerPrefs.SetInt("FreeSpinUsed", 1);
            }*/
            _touchTaskFreeSpin.SetActive(false);
            SoundPlayer.Instance.PlayButtonClick();
            _dailyTimerADSFortune.StartButtonClick();
            _ads.ShowRewarded(() =>
            {
                // AppMetrica.ReportEvent("RewardAD", "{\"" + "FortuneSpinADS" + "\":null}");
                Spin();
            });
        }

        private void AnimateButton()
        {
            if (_spinFreeButton.activeSelf)
            {
                _fortuneButton.enabled = true;
            }
            else
            {
                _fortuneButton.enabled = false;
                _fortuneButton.transform.localScale = Vector3.one;
            }
        }

        private void Spin()
        {
            _backWinText.SetActive(false);
            _spinWheelController.TurnWheel();
            _dailyTimerFortune.UpdateInfo();
        }

        private void ActivateFreeSpinButton()
        {
            Debug.Log("ActivateFreeSpinButton");
            DeactivationButtons();
            _spinFreeButton.SetActive(true);
        }

        private void ActivateOpenFortuneButton(int playerLevel)
        {
            // _openFortuneButton.interactable = playerLevel >= 3;
            
            _openFortuneButton.gameObject.SetActive(playerLevel >= 3);
        }

        private void ActiveOtherSpinButton()
        {
            Debug.Log("ActiveOtherSpinButton");
            DeactivationButtons();

            if (_currentValueSpin <= 0)
            {
                Debug.Log("_currentValueSpin " + _currentValueSpin);
                _dailyTimerADSFortune.UpdateInfo();
            }
            else
            {
                Debug.Log("_spinValueButton TRUE ");
                _spinValueButton.SetActive(true);
            }
        }

        private void DeactivationButtons()
        {
            foreach (var button in _spinButtons)
                button.SetActive(false);
        }

        private void SetPrize(int index)
        {
            Debug.Log(index);

            Prize prize = prizeMap[index];
            /*_backWinText.SetActive(true);
            _prizeText.text = $"You Win: + {prize.Value}  {prize.Type.ToString()}";*/

            /*if (!_isFreeButtonUsed)
            {
                AppMetrica.ReportEvent("TaskFortuna");
                FreeSpinUsed?.Invoke();
                PlayerPrefs.SetInt("FreeSpinUsed", 1);
            }*/
            _fortuneScreen.OpenPopupPrize(_fortuneSpriteConfig.GetSpriteByType(prize.Type), prize.Value);
            _soundPlayer.PlayFortunePrize();

            switch (prize.Type)
            {
                case PrizesFortune.Money:
                    _wallet.Add(new DollarValue(prize.Value, 0));
                    break;

                case PrizesFortune.XP:
                    _playerLevel.AddExp(prize.Value);
                    break;

                case PrizesFortune.Spin:
                    AddSpins(prize.Value);
                    break;
                
                case PrizesFortune.Energy:
                    _energy.IncreaseEnergy(prize.Value);
                    break;
            }
        }

        private void SaveSpinData()
        {
            SpinData data = new SpinData { currentValueSpin = _currentValueSpin };
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
            Debug.Log("сохрпанение " + data.currentValueSpin);
        }

        private void LoadSpinData()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SpinData data = JsonUtility.FromJson<SpinData>(json);
                _currentValueSpin = data.currentValueSpin;
                Debug.Log("Загрузка " + data.currentValueSpin);
            }
        }
        
        [ContextMenu("DeleteSpinData")]
        private void DeleteSpinData()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("Сохраненные данные удалены.");
            }
            else
            {
                Debug.Log("Файл сохраненных данных не найден.");
            }
        }
    }

    [System.Serializable]
    public class SpinData
    {
        public int currentValueSpin;
    }

    public struct Prize
    {
        public PrizesFortune Type;
        public int Value;
    }
}