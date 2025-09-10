using System;
using System.Collections;
using ADSContent;
using IAP;
using LoadingSceneContent;
using PlayerContent.LevelContent;
using SaveContent;
using UI.Buttons;
using UnityEngine;

namespace DisableInterContent
{
    public class DisablerInter : MonoBehaviour
    {
        private const string RewardKey = "currentValueShowRewardDisableInter";

        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private GameObject _buttonOpenDisableInterScreen;
        [SerializeField] private ADS _ads;
        [SerializeField] private DisableInterScreen _disableInterScreen;
        [SerializeField] private OpenScreenButton _openScreenButton;
        [SerializeField] private DisablerInterTimer _disablerInterTimer;
        [SerializeField] private DisableInterViewer _disableInterViewer;
        [SerializeField] private Animator _animator;
        [SerializeField] private LoadingGame _loadingGame;
        [SerializeField] private Purchaser _purchaser;

        private int _currentValueShowReward = 0;
        private bool _isActivateDisableInter = false;
        private Coroutine _autoSaveCoroutine;

        public event Action<int> CurrentValueChanged;
        public event Action StartTimerDisableInter;

        private void OnEnable()
        {
            _playerLevel.LevelChanged += SetValue;
            _disablerInterTimer.TimerCompleted += Reset;
            _loadingGame.MirraSDKInitialization += Init;
            _purchaser.RemoveADSPurchased += Deactivate;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= SetValue;
            _disablerInterTimer.TimerCompleted -= Reset;
            _loadingGame.MirraSDKInitialization -= Init;
            _purchaser.RemoveADSPurchased -= Deactivate;

            if (_autoSaveCoroutine != null) // ✅ останавливаем корутину при выключении объекта
                StopCoroutine(_autoSaveCoroutine);
        }

        /*private void Start()
        {
            Load();
            _autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
        }*/

        /*private void OnApplicationQuit()
        {
            Save();
            // PlayerPrefs.Save();
        }
        */

        private void Init()
        {
            bool removeAds = StorageHelper.GetInt("removeADS") == 1;

            Debug.Log("Init DisableInter" + removeAds);

            if (removeAds)
            {
                Deactivate();
                return;
            }

            Debug.Log("Init DisableInter");

            Load();
            _autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
        }

        private void Deactivate()
        {
            if (_disableInterScreen.gameObject.activeSelf)
                _disableInterScreen.CloseScreen();
            
            _buttonOpenDisableInterScreen.SetActive(false);
        }

        private IEnumerator AutoSaveRoutine()
        {
            var delay = new WaitForSeconds(60f);

            while (true)
            {
                yield return delay;

                if (_isActivateDisableInter)
                    Save();
            }
        }

        public void GetReward()
        {
            _ads.ShowRewarded(() =>
            {
                _currentValueShowReward++;

                if (_currentValueShowReward >= 3)
                    _currentValueShowReward = 3;

                Save();
                CurrentValueChanged?.Invoke(_currentValueShowReward);
                // AppMetrica.ReportEvent("RewardAD", "{\"" + "RewardAD_removeInter" + "\":null}");

                if (_currentValueShowReward > 2)
                {
                    SetAnimButton(false);
                    StartTimerDisableInter?.Invoke();
                    _isActivateDisableInter = true;
                    _openScreenButton.enabled = !_isActivateDisableInter;
                    _disableInterScreen.CloseScreen();
                    // AppMetrica.ReportEvent("RemoveInter_6h");
                }
            });
        }

        public void Reset()
        {
            SetAnimButton(true);
            _currentValueShowReward = 0;
            _isActivateDisableInter = false;
            _openScreenButton.enabled = !_isActivateDisableInter;
            CurrentValueChanged?.Invoke(_currentValueShowReward);
            Save();
        }

        private void SetValue(int level)
        {
            bool removeAds = StorageHelper.GetInt("removeADS") == 1;

            if (removeAds)
                return;

            _buttonOpenDisableInterScreen.SetActive(level >= 2);
        }

        private void Save()
        {
            // PlayerPrefs.SetInt("currentValueShowRewardDisableInter", _currentValueShowReward);
            StorageHelper.SetInt(RewardKey, _currentValueShowReward);
        }

        private void Load()
        {
            // _currentValueShowReward = PlayerPrefs.GetInt("currentValueShowRewardDisableInter", 0);
            _currentValueShowReward = StorageHelper.GetInt(RewardKey, 0);

            if (_currentValueShowReward > 2)
            {
                SetAnimButton(false);
                _disableInterViewer.ActivateTimer();
                _isActivateDisableInter = true;
                _openScreenButton.enabled = !_isActivateDisableInter;
            }

            SetValue(_playerLevel.CurrentLevel);
            _openScreenButton.enabled = !_isActivateDisableInter;
            CurrentValueChanged?.Invoke(_currentValueShowReward);
        }

        private void SetAnimButton(bool value)
        {
            _animator.enabled = value;

            if (!value)
                _buttonOpenDisableInterScreen.transform.localScale = Vector3.one;
        }
    }
}