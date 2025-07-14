using UnityEngine;

namespace ADSContent
{
    public class ADS : MonoBehaviour
    {
        /*[Header("Appodeal")] [SerializeField] private string _keyAppodeal;
        [SerializeField] private bool _isAppodeal;

        [Space] [Header("Applovin")] public string SDKKey =
            "nR3VEu5EEJlq6OmqwXd1lMKHQhg3sEumJpdTgplZ-csu1yq6zIkU1auq9P1sOOoIVLg9tOWSXDaUfRvC9Uv-Ib";

        public string InterstitialKey;
        public string RewardedKey;
        public string BannerKey;
        private bool isInitialized = false;

        private RewardCallback currentRewardCallback;
        private Coroutine _reloadInterstitialCoroutine;
        private bool _isInterstitialLoading = false;
        private bool _isAdPreloading = false;
        private int _interstitialRetryAttempt = 0;
        private bool _showInter = true;
        private bool _temporaryStopInters = false;

        public delegate void RewardCallback();

        public bool IsInterstitialReady => MaxSdk.IsInterstitialReady(InterstitialKey);

        public event Action _interHidden;

        /*private void Awake()
        {
            bool removeAds = PlayerPrefs.GetInt("removeADS") == 1;
            SetValue(!removeAds);

            if (_isAppodeal)
                Initialize(_keyAppodeal);
            else
                Init();
        }#1#


        /*private void Start()
        {
            bool removeAds = PlayerPrefs.GetInt("removeADS") == 1;
            SetValue(!removeAds);

            if (_isAppodeal)
                Initialize(_keyAppodeal);
            else
                Init();
        }#1#

        private void Init()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                Debug.Log("AppLovin successfully initialized");
            };

            MaxSdk.SetSdkKey(SDKKey);
            MaxSdk.InitializeSdk();

            InitializeInterstitialAds();
            InitializeRewardedAds();
        }

        public void SetValue(bool value)
        {
            _showInter = value;
        }

        public void SetTemporaryIntersValue(bool value)
        {
            _temporaryStopInters = value;
        }

        public void InitializeInterstitialAds()
        {
            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            // Load the first interstitial
            LoadInterstitial();
        }

        private void LoadInterstitial()
        {
            if (_isInterstitialLoading || MaxSdk.IsInterstitialReady(InterstitialKey))
                return;

            _isInterstitialLoading = true;

            MaxSdk.LoadInterstitial(InterstitialKey);
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            _isInterstitialLoading = false;
            _interstitialRetryAttempt = 0;
            AppMetrica.ReportEvent("OnInterstitialLoadedEvent");
        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            AppMetrica.ReportEvent("OnInterstitialLoadFailedEvent");
            _isInterstitialLoading = false;

            _interstitialRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, _interstitialRetryAttempt));

            if (_reloadInterstitialCoroutine != null)
                StopCoroutine(_reloadInterstitialCoroutine);

            _reloadInterstitialCoroutine = StartCoroutine(ReloadInterstitialAfterDelay((float)retryDelay));
            // LoadInterstitial();
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            AppMetrica.ReportEvent("OnInterstitialDisplayedEvent");
        }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            AppMetrica.ReportEvent("OnInterstitialAdFailedToDisplayEvent");
            // Interstitial ad failed to display. AppLovin recommends that you load the next ad.

            if (_reloadInterstitialCoroutine != null)
                StopCoroutine(_reloadInterstitialCoroutine);

            _reloadInterstitialCoroutine = StartCoroutine(ReloadInterstitialAfterDelay(0.5f));
        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            AppMetrica.ReportEvent("OnInterstitialClickedEvent");
        }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            _interHidden?.Invoke();
            AppMetrica.ReportEvent("OnInterstitialHiddenEvent");
            // Interstitial ad is hidden. Pre-load the next ad.

            if (_reloadInterstitialCoroutine != null)
                StopCoroutine(_reloadInterstitialCoroutine);

            _reloadInterstitialCoroutine = StartCoroutine(ReloadInterstitialAfterDelay(0.5f));

            // LoadInterstitial();
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

            if (_isAppodeal)
            {
                if (Appodeal.IsLoaded(AppodealAdType.Interstitial))
                {
                    Appodeal.Show(AppodealShowStyle.Interstitial);
                }
                /*else if (!Appodeal.IsAutoCacheEnabled(AppodealAdType.Interstitial))
                {
                    Appodeal.Cache(AppodealAdType.Interstitial);
                }
                else
                {
                    Appodeal.Cache(AppodealAdType.Interstitial);
                }#1#
                
            }
            else
            {
                if (MaxSdk.IsInterstitialReady(InterstitialKey))
                {
                    AppMetrica.ReportEvent("ShowInterstitial");
                    MaxSdk.ShowInterstitial(InterstitialKey);
                }
                else
                {
                    LoadInterstitial();
                }
            }
        }

        private IEnumerator ReloadInterstitialAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            LoadInterstitial();
        }


        public void InitializeRewardedAds()
        {
            // Attach callback
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first rewarded ad
            LoadRewardedAd();
        }

        private void LoadRewardedAd()
        {
            MaxSdk.LoadRewardedAd(RewardedKey);
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdLoadedEvent");
            //
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            Debug.Log("OnRewardedAdLoadFailedEvent");
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdDisplayedEvent");
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdFailedToDisplayEvent");
            // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
            LoadRewardedAd();
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdClickedEvent");
        }

        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdHiddenEvent");
            // Rewarded ad is hidden. Pre-load the next ad
            LoadRewardedAd();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdReceivedRewardEvent");

            if (currentRewardCallback != null)
            {
                currentRewardCallback();
                currentRewardCallback = null; //// Вызываем делегат
            }
            // The rewarded ad displayed and the user should receive the reward.
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("OnRewardedAdRevenuePaidEvent");
            // Ad revenue paid. Use this callback to track user revenue.
        }

        public void ShowRewarded(RewardCallback rewardCallback)
        {
            if (_isAppodeal)
            {
                if (Appodeal.IsLoaded(AppodealAdType.RewardedVideo))
                {
                    currentRewardCallback = rewardCallback;
                    Appodeal.Show(AppodealShowStyle.RewardedVideo);
                }
            }
            else
            {
                if (MaxSdk.IsRewardedAdReady(RewardedKey))
                {
                    currentRewardCallback = rewardCallback;
                    MaxSdk.ShowRewardedAd(RewardedKey);
                }
            }
        }


        //ТУТ НАЧАЛО APPODEAL
        //ТУТ НАЧАЛО APPODEAL
        //ТУТ НАЧАЛО APPODEAL
        //ТУТ НАЧАЛО APPODEAL
        //ТУТ НАЧАЛО APPODEAL


        private void Initialize(string key)
        {
            /*Appodeal.SetAutoCache(AppodealAdType.RewardedVideo, true);
            Appodeal.SetAutoCache(AppodealAdType.Interstitial, true);#1#

            // Appodeal.SetTesting(true); // Включить тестовые объявления
            // Appodeal.SetLogLevel(AppodealLogLevel.Verbose);


            int adTypes = AppodealAdType.Interstitial | AppodealAdType.RewardedVideo;
            string appKey = key;
            AppodealCallbacks.Sdk.OnInitialized += OnInitializationFinished;
            Appodeal.Initialize(appKey, adTypes);
            SomeMethod();
        }

        public void SomeMethod()
        {
            AppodealCallbacks.Interstitial.OnLoaded += OnInterstitialLoaded;
            AppodealCallbacks.Interstitial.OnFailedToLoad += OnInterstitialFailedToLoad;
            AppodealCallbacks.Interstitial.OnShown += OnInterstitialShown;
            AppodealCallbacks.Interstitial.OnShowFailed += OnInterstitialShowFailed;
            AppodealCallbacks.Interstitial.OnClosed += OnInterstitialClosed;
            AppodealCallbacks.Interstitial.OnClicked += OnInterstitialClicked;
            AppodealCallbacks.Interstitial.OnExpired += OnInterstitialExpired;

            AppodealCallbacks.RewardedVideo.OnLoaded += OnRewardedVideoLoaded;
            AppodealCallbacks.RewardedVideo.OnFailedToLoad += OnRewardedVideoFailedToLoad;
            AppodealCallbacks.RewardedVideo.OnShown += OnRewardedVideoShown;
            AppodealCallbacks.RewardedVideo.OnShowFailed += OnRewardedVideoShowFailed;
            AppodealCallbacks.RewardedVideo.OnClosed += OnRewardedVideoClosed;
            AppodealCallbacks.RewardedVideo.OnFinished += OnRewardedVideoFinished;
            AppodealCallbacks.RewardedVideo.OnClicked += OnRewardedVideoClicked;
            AppodealCallbacks.RewardedVideo.OnExpired += OnRewardedVideoExpired;
        }

        public void OnInitializationFinished(object sender, SdkInitializedEventArgs e)
        {
            Debug.Log("Appodeal Initialized!");
        }

        private void OnInterstitialLoaded(object sender, AdLoadedEventArgs e)
        {
            Debug.Log("Interstitial loaded");
        }

        private int _interstitialRetryCount = 0;

        private void OnInterstitialFailedToLoad(object sender, EventArgs e)
        {
            Debug.Log("Interstitial failed to load");

            /*_interstitialRetryCount++;
            float delay = Mathf.Pow(2, Mathf.Min(5, _interstitialRetryCount));
            StartCoroutine(RetryLoadInterstitial(delay));#1#
        }

        /*private IEnumerator RetryLoadInterstitial(float delay)
        {
            yield return new WaitForSeconds(delay);
            Appodeal.Cache(AppodealAdType.Interstitial);
        }#1#

        private void OnInterstitialShowFailed(object sender, EventArgs e)
        {
            Debug.Log("Interstitial show failed");
        }

        private void OnInterstitialShown(object sender, EventArgs e)
        {
            Debug.Log("Interstitial shown");
        }

        private void OnInterstitialClosed(object sender, EventArgs e)
        {
            Debug.Log("Interstitial closed");
        }

        private void OnInterstitialClicked(object sender, EventArgs e)
        {
            Debug.Log("Interstitial clicked");
        }

        private void OnInterstitialExpired(object sender, EventArgs e)
        {
            Debug.Log("Interstitial expired");
        }

        private void OnRewardedVideoLoaded(object sender, AdLoadedEventArgs e)
        {
            Debug.Log($"[APDUnity] [Callback] OnRewardedVideoLoaded(bool isPrecache:{e.IsPrecache})");
        }

// Called when rewarded video failed to load
        private void OnRewardedVideoFailedToLoad(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoFailedToLoad()");
        }

        private void OnRewardedVideoShowFailed(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoShowFailed()");
        }

        private void OnRewardedVideoShown(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoShown()");
        }

        private void OnRewardedVideoClosed(object sender, RewardedVideoClosedEventArgs e)
        {
            Debug.Log($"[APDUnity] [Callback] OnRewardedVideoClosed(bool finished:{e.Finished})");
        }

// Called when rewarded video is viewed until the end
        private void OnRewardedVideoFinished(object sender, RewardedVideoFinishedEventArgs e)
        {
            if (currentRewardCallback != null)
            {
                currentRewardCallback();
                currentRewardCallback = null; //// Вызываем делегат
            }

            Debug.Log(
                $"[APDUnity] [Callback] OnRewardedVideoFinished(double amount:{e.Amount}, string name:{e.Currency})");
        }

        private void OnRewardedVideoClicked(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoClicked()");
        }

        private void OnRewardedVideoExpired(object sender, EventArgs e)
        {
            Debug.Log("[APDUnity] [Callback] OnRewardedVideoExpired()");
        }*/
    }
}