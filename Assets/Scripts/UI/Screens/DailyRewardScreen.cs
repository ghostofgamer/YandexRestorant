using DailyRewardContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class DailyRewardScreen : AbstractScreen
    {
        private const string LastClaimDateKey = "LastClaimDate";
        private const string CurrentDayIndexKey = "CurrentDayIndex";

        [SerializeField] private DailyReward _dailyReward;
        [SerializeField] private Button[] _dailyButtons;
        [SerializeField] private GameObject[] _checkMarks;
        [SerializeField] private Sprite _rewardClaimedSprite;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _currentDaySprite;
        [SerializeField] private Sprite _lastDayDefaultSprite;
        [SerializeField] private GameObject _superPrizeDecoration;
        [SerializeField] private GameObject _superPrizeMoney;
    }
}