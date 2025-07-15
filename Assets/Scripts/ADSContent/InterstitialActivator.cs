using System;
using UnityEngine;

namespace ADSContent
{
    public class InterstitialActivator : MonoBehaviour
    {
        private static InterstitialActivator _instance;
        private const string LastADKey = "LastAdInterShow";
        private const string FirstLaunchKey = "FirstLaunchTime";
        
        [SerializeField] private ADS _ads;
        [SerializeField] private float _duration; 
        
        private TimeSpan adCooldown;
        private DateTime _sessionStartTime;

        public static InterstitialActivator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InterstitialActivator>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("AdManager");
                        _instance = obj.AddComponent<InterstitialActivator>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return _instance;
            }
        }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSession();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            adCooldown = TimeSpan.FromMinutes(_duration);
        }
        
        /*public static InterstitialActivator Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("AdManager");
                    _instance = obj.AddComponent<InterstitialActivator>();
                    DontDestroyOnLoad(obj);
                }

                return _instance;
            }
        }

        void Awake()
        {
            adCooldown = TimeSpan.FromMinutes(_duration);
            
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSession();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }*/

        private void InitializeSession()
        {
            _sessionStartTime = DateTime.UtcNow;
            
            if (!PlayerPrefs.HasKey(FirstLaunchKey))
                PlayerPrefs.SetString(FirstLaunchKey, _sessionStartTime.Ticks.ToString());
        }

        public void ShowAd()
        {
            if (CanShowAd())
            {
                _ads.ShowInterstitial();
                // Debug.Log("$$$Showing Ad");
                PlayerPrefs.SetString(LastADKey, DateTime.UtcNow.Ticks.ToString());
                PlayerPrefs.Save();
            }
            else
            {
                // Debug.Log("$$$Ad cooldown active");
            }
        }

        private bool CanShowAd()
        {
            DateTime currentTime = DateTime.UtcNow;
            
            if (PlayerPrefs.HasKey(FirstLaunchKey))
            {
                long firstLaunchTicks = long.Parse(PlayerPrefs.GetString(FirstLaunchKey));
                DateTime firstLaunchTime = new DateTime(firstLaunchTicks, DateTimeKind.Utc);

                if ((currentTime - firstLaunchTime) < adCooldown)
                {
                    Debug.Log("Ad not ready: first launch cooldown");
                    return false;
                }
            }
            
            if ((currentTime - _sessionStartTime) < adCooldown)
            {
                Debug.Log("Ad not ready: session cooldown");
                return false;
            }

            if (PlayerPrefs.HasKey(LastADKey))
            {
                long lastAdTicks = long.Parse(PlayerPrefs.GetString(LastADKey));
                DateTime lastAdTime = new DateTime(lastAdTicks, DateTimeKind.Utc);

                if ((currentTime - lastAdTime) < adCooldown)
                {
                    Debug.Log("Ad not ready: interval cooldown");
                    return false;
                }
            }

            return true;
        }
    }
}