using System;
using System.Collections;
using ADSContent;
using UnityEngine;

namespace DisableInterContent
{
    public class DisablerInterTimer : MonoBehaviour
    {
        private const string TIMER_SAVE_KEY = "InterTimerEnd";

        [SerializeField] private DisablerInter _disablerInter;
        [SerializeField] private float _durationSeconds;
        [SerializeField] private InterstitialTimer _interstitialTimer;
        [SerializeField] private ADS _ads;

        private bool _isTimerActive = false;
        private DateTime _endTime;
        private Coroutine _timerCoroutine;

        public event Action<TimeSpan> TimeChanged;

        public event Action TimerCompleted;

        private void OnEnable()
        {
            _disablerInter.StartTimerDisableInter += Activate;
        }

        private void OnDisable()
        {
            _disablerInter.StartTimerDisableInter -= Activate;
        }

        private void Start()
        {
            LoadTimer();
        }

        private void OnApplicationQuit()
        {
            SaveTimer();
            PlayerPrefs.Save();
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveTimer();
                PlayerPrefs.Save();
            }
            else
            {
                LoadTimer();
            }
        }

        private void Activate()
        {
            // _interstitialTimer.SetTemporaryIntersValue(true);
            _ads.SetTemporaryIntersValue(true);
            _isTimerActive = true;

            _endTime = DateTime.Now.AddSeconds(_durationSeconds);
            SaveTimer();

            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);

            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            while (DateTime.Now < _endTime)
            {
                UpdateTimerDisplay();
                yield return new WaitForSeconds(1);
            }

            OnTimerComplete();
        }

        private void OnTimerComplete()
        {
            _isTimerActive = false;
            // _interstitialTimer.SetTemporaryIntersValue(false);
            _ads.SetTemporaryIntersValue(false);
            Debug.Log("TimerCompleted!!!!!!!!!!!!!!!!!!!!");
            TimerCompleted?.Invoke();
            PlayerPrefs.DeleteKey(TIMER_SAVE_KEY);
            PlayerPrefs.Save();
        }

        private void UpdateTimerDisplay()
        {
            TimeSpan remaining = _endTime - DateTime.Now;
            TimeChanged?.Invoke(remaining);
        }

        private void SaveTimer()
        {
            if (_isTimerActive)
                PlayerPrefs.SetString(TIMER_SAVE_KEY, _endTime.ToString("O"));
        }

        private void LoadTimer()
        {
            if (PlayerPrefs.HasKey(TIMER_SAVE_KEY))
            {
                _endTime = DateTime.Parse(PlayerPrefs.GetString(TIMER_SAVE_KEY));

                if (DateTime.Now < _endTime)
                {
                    _isTimerActive = true;
                    // _interstitialTimer.SetTemporaryIntersValue(true);
                    _ads.SetTemporaryIntersValue(true);
                    UpdateTimerDisplay();

                    if (_timerCoroutine != null)
                        StopCoroutine(_timerCoroutine);

                    _timerCoroutine = StartCoroutine(TimerCoroutine());
                }
                else
                {
                    OnTimerComplete();
                }
            }
            else
            {
                _isTimerActive = false;
                _ads.SetTemporaryIntersValue(false);
                // _interstitialTimer.SetTemporaryIntersValue(false);
            }
        }
    }
}