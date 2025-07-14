using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace DisableInterContent
{
    public class DisableInterViewer : MonoBehaviour
    {
        [SerializeField] private DisablerInter _disablerInter;
        [SerializeField] private Image _fillImage;
        [SerializeField] private RewardAdDisableInterLockChanger[] _rewardAdDisableInterLockChangers;
        [SerializeField] private GameObject _timeContent;
        [SerializeField] private TMP_Text _timeValueText;
        [SerializeField] private DisablerInterTimer _disablerInterTimer;
        
        private void OnEnable()
        {
            _disablerInter.CurrentValueChanged += UpdateUI;
            _disablerInter.StartTimerDisableInter += ActivateTimer;
            _disablerInterTimer.TimeChanged += TimeChange;
            _disablerInterTimer.TimerCompleted += Reset;
        }

        private void OnDisable()
        {
            _disablerInter.CurrentValueChanged -= UpdateUI;
            _disablerInterTimer.TimeChanged -= TimeChange;
            _disablerInterTimer.TimerCompleted -= Reset;
            _disablerInter.StartTimerDisableInter -= ActivateTimer;
        }

        private void UpdateUI(int currentValue)
        {
            _fillImage.fillAmount = currentValue switch
            {
                0 => 0f,
                1 => 0.5f,
                2 => 1f,
                3 => 1f,
                _ => _fillImage.fillAmount
            };

            foreach (var rewardAdDisableInterLockChanger in _rewardAdDisableInterLockChangers)
                rewardAdDisableInterLockChanger.SetValue(currentValue);
        }

        public void ActivateTimer()
        {
            _timeContent.SetActive(true);
        }
        
        private void TimeChange(TimeSpan remaining)
        {
            _timeValueText.text = $"{remaining.Hours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}";
        }

        private void Reset()
        {
            _timeValueText.text = "00:00:00";
            _timeContent.SetActive(false);
        }
    }
}