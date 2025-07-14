using System;
using Enums;
using TutorialContent;
using UI.Screens.AdsScreens;
using UnityEngine;

namespace ADSContent
{
    public class InterstitialTimer : MonoBehaviour
    {
        [SerializeField] private ADS _ads;
        [SerializeField] private float interval;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private RemoveAdScreen _removeAdScreen;

        private float timer = 0f;
        private DateTime lastAdTime;
        private bool _showInter = true;
        private int adShowCount = 0;
        private bool _isPaused = false;
        private bool _temporaryStopInters = false;
        private float _nextCheckTime;
        
        private void Start()
        {
            bool removeAds = PlayerPrefs.GetInt("removeADS") == 1;
            SetValue(!removeAds);

            lastAdTime = DateTime.Now;
        }

        private void Update()
        {
            if (_temporaryStopInters)
                return;
            
            if (!_showInter)
                return;

            if (_isPaused)
                return;

            timer += Time.deltaTime;

            if (timer >= interval)
            {
                if (_ads.IsInterstitialReady)
                {
                    ShowInterstitial();
                    ResetTimer();
                }
                else if (Time.time >= _nextCheckTime)
                {
                    CheckAdReadiness();
                    _nextCheckTime = Time.time + 5f; // Следующая проверка через 5 сек
                }
                
                /*ShowInterstitial();
                timer = 0f;
                lastAdTime = DateTime.Now;*/
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            _isPaused = pauseStatus;
            Time.timeScale = pauseStatus ? 0 : 1;
        }

        public void SetValue(bool value)
        {
            _showInter = value;
        }

        public void SetTemporaryIntersValue(bool value)
        {
            _temporaryStopInters = value;
        }

        private void ShowInterstitial()
        {
            _ads.ShowInterstitial();

            adShowCount++;

            if (adShowCount >= 2)
            {
                _removeAdScreen.OpenScreen();
                adShowCount = 0;
            }
        }
        
        private void CheckAdReadiness()
        {
            if (_ads.IsInterstitialReady)
            {
                ShowInterstitial();
                ResetTimer();
            }
            else if (timer >= interval + 60f) // Превысили максимальное время ожидания
            {
                ResetTimer(); // Сбрасываем, чтобы попробовать через 2 минуты
            }
        }
        
        private void ResetTimer()
        {
            timer = 0f;
            lastAdTime = DateTime.Now;
        }
    }
}