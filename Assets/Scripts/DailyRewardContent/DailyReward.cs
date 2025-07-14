using System;
using Io.AppMetrica;
using SettingsContent.SoundContent;
using UnityEngine;
using UnityEngine.UI;

namespace DailyRewardContent
{
    public class DailyReward : MonoBehaviour
    {
        private const string LastClaimDateKey = "LastClaimDate";
        private const string CurrentDayIndexKey = "CurrentDayIndex";

        [SerializeField] private Prize _dailyReward;
        [SerializeField] private Button[] _dayButtons;
        [SerializeField] private Button _claimButton;
        [SerializeField] private GameObject[] _checkMarks;
        [SerializeField] private Color _rewardClaimedSprite;
        [SerializeField] private Color _defaultSprite;
        [SerializeField] private Color _currentDaySprite;
        [SerializeField] private Color _lastDayDefaultSprite;
        [SerializeField] private Animator _openButtonAnimation;
        [SerializeField] private GameObject[] _prizes;

        private int _currentDayIndex;
        private bool _rewardClaimedToday;

        public int _testDaysOffset;
        public bool _isTesting;

        public event Action ClaimTodayCompleted;

        private void Start()
        {
            // _decorationSystem = gui.GameWorld.DecorationSystem;
            InitializeDailyRewards();
        }

        private void InitializeDailyRewards()
        {
            DateTime lastClaimDate = GetLastClaimDate();
            // DateTime currentDate = DateTime.Now;
            DateTime currentDate = _isTesting ? DateTime.Now.AddDays(_testDaysOffset) : DateTime.Now;

            if (lastClaimDate == DateTime.MinValue)
            {
                _currentDayIndex = 0;
                _rewardClaimedToday = false;
            }
            else if ((currentDate - lastClaimDate).TotalDays >= 2)
            {
                _currentDayIndex = 0;
                _rewardClaimedToday = false;
                PlayerPrefs.SetInt(CurrentDayIndexKey, -1);
            }
            else if ((currentDate - lastClaimDate).TotalDays >= 1)
            {
                _currentDayIndex = PlayerPrefs.GetInt(CurrentDayIndexKey, 0);

                if (_currentDayIndex >= _dayButtons.Length - 1)
                    _currentDayIndex = 0;
                else
                    _currentDayIndex++;

                _rewardClaimedToday = false;
            }
            else
            {
                _currentDayIndex = PlayerPrefs.GetInt(CurrentDayIndexKey, 0);
                _rewardClaimedToday = true;
            }

            UpdateUI();
        }

        private DateTime GetLastClaimDate()
        {
            string lastClaimDateString = PlayerPrefs.GetString(LastClaimDateKey, string.Empty);

            if (string.IsNullOrEmpty(lastClaimDateString))
            {
                return DateTime.MinValue;
            }

            return DateTime.Parse(lastClaimDateString);
        }

        private void UpdateUI()
        {
            var currentIndex = PlayerPrefs.GetInt(CurrentDayIndexKey, -1);

            /*SetValueSuperPrizeIcon(
                _decorationSystem.GetActivationValueDecoration(_decorationSystem.CurrentDailyRewardDecoration));*/

            for (int i = 0; i < _dayButtons.Length; i++)
            {
                if (i < _currentDayIndex)
                {
                    /*_dayButtons[i].image.color = _rewardClaimedSprite;
                    _dayButtons[i].image.color = _rewardClaimedSprite;*/
                    _prizes[i].SetActive(false);
                    _checkMarks[i].SetActive(true);
                }
                else if (i == _currentDayIndex)
                {
                    _dayButtons[i].image.color = currentIndex == i ? _defaultSprite : _currentDaySprite;
                    _checkMarks[i].SetActive(currentIndex == i);
                    _prizes[i].SetActive(currentIndex != i);
                }
                else
                {
                    _dayButtons[i].image.color = i == _dayButtons.Length - 1 ? _lastDayDefaultSprite : _defaultSprite;
                    // _checkMarks[i].SetActive(false);
                }

                // dayButtons[i].interactable = (i == currentDayIndex && !rewardClaimedToday);
            }

            _claimButton.interactable = (_currentDayIndex < _dayButtons.Length && !_rewardClaimedToday);
            AnimateOpenScreenButton();
        }

        private void AnimateOpenScreenButton()
        {
            if (_currentDayIndex < _dayButtons.Length && !_rewardClaimedToday)
                _openButtonAnimation.enabled = true;
            else
            {
                _openButtonAnimation.enabled = false;
                _openButtonAnimation.transform.localScale = Vector3.one;
            }
        }

        public bool IsRewardDay()
        {
            DateTime lastClaimDate = GetLastClaimDate();
            // DateTime currentDate = DateTime.Now;
            DateTime currentDate = _isTesting ? DateTime.Now.AddDays(_testDaysOffset) : DateTime.Now;

            if (lastClaimDate == DateTime.MinValue)
            {
                _currentDayIndex = 0;
                _rewardClaimedToday = false;
            }
            else if ((currentDate - lastClaimDate).TotalDays >= 2)
            {
                _currentDayIndex = 0;
                _rewardClaimedToday = false;
                PlayerPrefs.SetInt(CurrentDayIndexKey, -1);
            }
            else if ((currentDate - lastClaimDate).TotalDays >= 1)
            {
                _currentDayIndex = PlayerPrefs.GetInt(CurrentDayIndexKey, 0);

                if (_currentDayIndex >= _dayButtons.Length - 1)
                    _currentDayIndex = 0;
                else
                    _currentDayIndex++;

                _rewardClaimedToday = false;
            }
            else
            {
                _currentDayIndex = PlayerPrefs.GetInt(CurrentDayIndexKey, 0);
                _rewardClaimedToday = true;
            }

            return (_currentDayIndex < _dayButtons.Length && !_rewardClaimedToday);
        }

        private void SetValueSuperPrizeIcon(bool value)
        {
            /*_superPrizeDecoration.SetActive(value);
            _superPrizeMoney.SetActive(!value);*/
        }

        public void ClaimReward()
        {
            DateTime lastClaimDate = GetLastClaimDate();
            // DateTime currentDate = DateTime.Now;
            DateTime currentDate = _isTesting ? DateTime.Now.AddDays(_testDaysOffset) : DateTime.Now;
            SoundPlayer.Instance.PlayButtonClick();

            if ((currentDate - lastClaimDate).TotalDays >= 1)
            {
                if (_currentDayIndex < _dayButtons.Length)
                {
                    Debug.Log("Награда за день " + (_currentDayIndex + 1) + " получена!");
                    AppMetrica.ReportEvent("DailyReward", "{\"" + (_currentDayIndex + 1).ToString() + "\":null}");
                    SoundPlayer.Instance.PlayDailyReward();
                    PlayerPrefs.SetString(LastClaimDateKey, DateTime.Now.ToString());
                    PlayerPrefs.SetInt(CurrentDayIndexKey, _currentDayIndex);
                    _rewardClaimedToday = true;
                    UpdateUI();
                    _dailyReward.Claim(_currentDayIndex);
                    ClaimTodayCompleted?.Invoke();
                }
            }
            else
            {
                Debug.Log("Вы уже получили награду за сегодня. Попробуйте завтра.");
            }
        }
    }
}