using System;
using Enums;
using PlayerContent.LevelContent;
using SettingsContent.SoundContent;
using SoContent;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;
using WorkerContent.Upgrade;

namespace UI.Screens.ShopContent.WorkersContent
{
    public class WorkerUIProduct : MonoBehaviour
    {
        public const string Worker = "Worker";

        [SerializeField] private GameObject PayContent;
        [SerializeField] private GameObject UpdateContent;
        [SerializeField] private Image _icon;
        [SerializeField] private WorkerType _workerType;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private GameObject _hireButton;
        [SerializeField] private GameObject _dismissButton;
        [SerializeField] private GameObject _requiredContent;
        [SerializeField] private GameObject _upgradeContent;
        [SerializeField] private GameObject _maxLevelContent;
        [SerializeField] private WorkerUpgrade _workerUpgrade;
        [SerializeField] private WorkerParametersConfig _workerParametersConfig;
        [SerializeField] private int _levelOpened;
        [SerializeField] private PlayerLevel _playerLevel;

        private int _level;
        public WorkerParameterConfig CurrentConfig { get; private set; }

        public event Action<DollarValue, DollarValue> ValueChanged;
        public event Action<WorkerType> WorkerBuyed;
        public event Action<WorkerType> WorkerFired;

        public event Action<WorkerParameterConfig> ParametersValueChanged;

        public bool IsOwned { get; private set; }
        public DollarValue Price { get; private set; }
        public DollarValue Salary { get; private set; }
        public WorkerType WorkerType => _workerType;
        public int LevelOpened => _levelOpened;

        private void Start()
        {
            if (PlayerPrefs.GetInt("Zona" + ZoneType.StaffRoom, 0) <= 0 || _playerLevel.CurrentLevel < _levelOpened)
            {
                ClosePurchased();
                return;
            }
            /*else if (PlayerPrefs.GetInt("Zona" + ZoneType.StaffRoom, 0) <= 0 ||
                     _playerLevel.CurrentLevel >= _levelOpened)
            {
                return;
            }*/

            IsOwned = PlayerPrefs.GetInt(Worker + _workerType, 0) > 0;
            SetValue();
        }

        public void Init(WorkerConfig workerConfig)
        {
            Debug.Log("InitWorkersG");

            InitUpgradeInfo();

            Price = workerConfig.Price;
            Salary = workerConfig.Salary;
            _icon.sprite = workerConfig.SpriteIcon;

            ValueChanged?.Invoke(Price, Salary);

            if (PlayerPrefs.GetInt("Zona" + ZoneType.StaffRoom, 0) <= 0 || _playerLevel.CurrentLevel < _levelOpened)
            {
                ClosePurchased();
                return;
            }
            /*else if (PlayerPrefs.GetInt("Zona" + ZoneType.StaffRoom, 0) <= ||
                     _playerLevel.CurrentLevel >= _levelOpened)
            {
                return;
            }*/
            else
            {
                IsOwned = PlayerPrefs.GetInt(Worker + _workerType, 0) > 0;
                SetValue();
            }
        }

        public void InitUpgradeInfo()
        {
            _level = PlayerPrefs.GetInt(_workerType + "LevelWorker", 1);
            CurrentConfig = _workerParametersConfig.GetConfig(_workerType, _level);
            ParametersValueChanged?.Invoke(CurrentConfig);
            SetValue();
        }

        public void BuyWorker()
        {
            if (_wallet.DollarValue.ToTotalCents() < Price.ToTotalCents())
                Debug.Log("недостаточно денег");

            // AppMetrica.ReportEvent("WorkerBuyed", "{\"" + _workerType.ToString() + "\":null}");
            SoundPlayer.Instance.PlayPayment();
            WorkerBuyed?.Invoke(_workerType);
            _wallet.Subtract(Price);
            IsOwned = true;
            PlayerPrefs.SetInt(Worker + _workerType, 1);
            SetValue();
        }

        private void SetValue()
        {
            _requiredContent.SetActive(false);
            PayContent.SetActive(!IsOwned);
            UpdateContent.SetActive(IsOwned);
            _hireButton.SetActive(!IsOwned);
            _dismissButton.SetActive(IsOwned);
            _upgradeContent.SetActive(IsOwned && CurrentConfig.Level < CurrentConfig.MaxLevel);
            _maxLevelContent.SetActive(IsOwned && CurrentConfig.Level >= CurrentConfig.MaxLevel);
        }

        public void DismissWorker()
        {
            WorkerFired?.Invoke(_workerType);
            IsOwned = false;
            PlayerPrefs.SetInt(Worker + _workerType, 0);
            SetValue();
        }

        private void ClosePurchased()
        {
            PayContent.SetActive(false);
            UpdateContent.SetActive(false);
            _hireButton.SetActive(false);
            _dismissButton.SetActive(false);
            _requiredContent.SetActive(true);
        }
    }
}