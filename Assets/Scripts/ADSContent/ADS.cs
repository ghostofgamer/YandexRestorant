using System;
using UnityEngine;

namespace ADSContent
{
    public class ADS : MonoBehaviour
    {
        private RewardCallback currentRewardCallback;
        private Coroutine _reloadInterstitialCoroutine;
        private bool _isInterstitialLoading = false;
        private bool _isAdPreloading = false;
        private int _interstitialRetryAttempt = 0;
        private bool _showInter = true;
        private bool _temporaryStopInters = false;

        public delegate void RewardCallback();

        public event Action _interHidden;

        private void Awake()
        {
            bool removeAds = PlayerPrefs.GetInt("removeADS") == 1;
            SetValue(!removeAds);
        }

        public void SetValue(bool value)
        {
            _showInter = value;
        }

        public void SetTemporaryIntersValue(bool value)
        {
            _temporaryStopInters = value;
        }
        
        public void ShowInterstitial()
        {
            if (_temporaryStopInters)
            {
                return;
            }

            if (!_showInter)
            {
                return;
            }
        }


        public void ShowRewarded(RewardCallback rewardCallback)
        {
            currentRewardCallback = rewardCallback;
        }
    }
}