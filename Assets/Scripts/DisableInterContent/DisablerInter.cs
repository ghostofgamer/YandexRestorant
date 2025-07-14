using System;
using System.Text;
using ADSContent;
using Io.AppMetrica;
using PlayerContent.LevelContent;
using UI.Buttons;
using UnityEngine;

namespace DisableInterContent
{
    public class DisablerInter : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private GameObject _buttonOpenDisableInterScreen;
        [SerializeField] private ADS _ads;
        [SerializeField] private DisableInterScreen _disableInterScreen;
        [SerializeField] private OpenScreenButton _openScreenButton;
        [SerializeField] private DisablerInterTimer _disablerInterTimer;
        [SerializeField] private DisableInterViewer _disableInterViewer;
        [SerializeField] private Animator _animator;

        private int _currentValueShowReward = 0;
        private bool _isActivateDisableInter = false;

        public event Action<int> CurrentValueChanged;

        public event Action StartTimerDisableInter;

        private void OnEnable()
        {
            _playerLevel.LevelChanged += SetValue;
            _disablerInterTimer.TimerCompleted += Reset;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= SetValue;
            _disablerInterTimer.TimerCompleted -= Reset;
        }

        private void Start()
        {
            Load();
        }

        private void OnApplicationQuit()
        {
            Save();
            PlayerPrefs.Save();
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
                AppMetrica.ReportEvent("RewardAD", "{\"" + "RewardAD_removeInter" + "\":null}");

                if (_currentValueShowReward > 2)
                {
                    SetAnimButton(false);
                    StartTimerDisableInter?.Invoke();
                    _isActivateDisableInter = true;
                    _openScreenButton.enabled = !_isActivateDisableInter;
                    _disableInterScreen.CloseScreen();
                    AppMetrica.ReportEvent("RemoveInter_6h");
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
            _buttonOpenDisableInterScreen.SetActive(level >= 2);
        }

        private void Save()
        {
            PlayerPrefs.SetInt("currentValueShowRewardDisableInter", _currentValueShowReward);
        }

        private void Load()
        {
            _currentValueShowReward = PlayerPrefs.GetInt("currentValueShowRewardDisableInter", 0);

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