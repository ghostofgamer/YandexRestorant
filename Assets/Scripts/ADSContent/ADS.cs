using System;
using MirraGames.SDK;
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
        private int _adShowCount = 0;

        public delegate void RewardCallback();

        public event Action _interHidden;

        public event Action RemoveAdsScreenOpening;

        private void Awake()
        {
            bool removeAds = PlayerPrefs.GetInt("removeADS") == 1;
            SetValue(!removeAds);
        }

        /*private void Start()
        {
            RestorePurchases();
        }*/

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
            bool isSDKInitialized = MirraSDK.IsInitialized;

            if (!isSDKInitialized)
            {
                Debug.LogWarning("SDK не инициализирована");
                return;
            }

            if (_temporaryStopInters)
            {
                return;
            }

            if (!_showInter)
            {
                return;
            }

            bool isInterstitialReady = MirraSDK.Ads.IsInterstitialReady;

            if (!isInterstitialReady)
            {
                Debug.LogWarning("Реклама Inter не готова к показу");
                return;
            }

            bool isInterstitialVisible = MirraSDK.Ads.IsInterstitialVisible;

            if (isInterstitialVisible)
            {
                Debug.LogWarning("Реклама Inter не готова к показу");
                return;
            }

            bool isInterstitialAvailable = MirraSDK.Ads.IsInterstitialAvailable;

            if (!isInterstitialAvailable)
            {
                Debug.LogWarning("Реклама Inter недоступна в текущем окружении");
                return;
            }

            MirraSDK.Ads.InvokeInterstitial(
                onOpen: () => Debug.Log("Межстраничная реклама открыта"),
                onClose: (isSuccess) =>
                {
                    _adShowCount++;

                    if (_adShowCount >= 2)
                    {
                        RemoveAdsScreenOpening?.Invoke();
                        _adShowCount = 0;
                    }

                    Debug.Log("Межстраничная реклама закрыта");
                });
        }


        public void ShowRewarded(RewardCallback rewardCallback)
        {
            bool isSDKInitialized = MirraSDK.IsInitialized;

            if (!isSDKInitialized)
            {
                Debug.LogWarning("SDK не инициализирована");
                return;
            }

            bool isRewardedReady = MirraSDK.Ads.IsRewardedReady;

            if (!isRewardedReady)
            {
                Debug.LogWarning("Реклама за вознаграждение не готова к показу");
                return;
            }

            bool isRewardedVisible = MirraSDK.Ads.IsRewardedVisible;

            if (isRewardedVisible)
            {
                Debug.LogWarning("Реклама за вознаграждение не готова к показу");
                return;
            }

            bool isRewardedAvailable = MirraSDK.Ads.IsRewardedAvailable;

            if (!isRewardedAvailable)
            {
                Debug.LogWarning("Реклама за вознаграждение недоступна в текущем окружении");
                return;
            }

            MirraSDK.Ads.InvokeRewarded(
                onSuccess: () =>
                {
                    rewardCallback?.Invoke();
                    Debug.Log("Реклама за вознаграждение успешно показана");
                },
                onOpen: () => Debug.Log("Реклама за вознаграждение открыта"),
                onClose: (isSuccess) => Debug.Log($"Реклама за вознаграждение закрыта с наградой '{isSuccess}'"),
                rewardTag: "extra_lives"
            );
        }

        /*public void RestorePurchases()
        {
            MirraSDK.Payments.RestorePurchases((restoreData) =>
            {
                // Список всех покупок игрока (содержит и выданные и невыданные товары)
                string[] allPurchases = restoreData.AllPurchases;
                Debug.Log($"Игрок совершил '{allPurchases.Length}' успешных покупок");

                // Список невыданных товаров, которые нужно выдать игроку (содержит уникальные теги товаров, т.е. если товар не выдан множество раз, он будет упомянут только один раз в массиве)
                string[] pendingProducts = restoreData.PendingProducts;
                Debug.Log(
                    $"Игрок не получил '{pendingProducts.Length}' разных товаров: [{string.Join(", ", pendingProducts)}]");

                // Выдать невыданные товары игроку
                // [пример кода ниже]
            });
        }*/
    }
}