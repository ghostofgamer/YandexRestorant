using System;
using CalendarContent;
using ClientsContent;
using EnergyContent;
using PlayerContent.LevelContent;
using RestaurantContent;
using TMPro;
using UI.Screens;
using UnityEngine;

namespace DayNightContent
{
    public class DayNightCycle : MonoBehaviour
    {
        private const string IS_DAY_KEY = "IsDay";
        private const string StatisticDay = "StatisticDay";
        private const string IS_NIGHT_KEY = "IsNight";
        private const string TIME_OF_DAY_KEY = "TimeOfDay";
        private const float START_HOUR = 9f;
        private const float END_HOUR = 22f;

        [SerializeField] private OpenCloseRestaurant _openCloseRestaurant;
        [SerializeField] private Material skyboxMaterial;
        [SerializeField] private float dayDuration;
        [SerializeField] private float _nightDuration;
        [SerializeField] private Color dayColor;
        [SerializeField] private Color nightColor;
        [SerializeField] private Color _dayAmbientColor;
        [SerializeField] private Color _nightAmbientColor;
        [SerializeField] private Color _dayEquatorColor;
        [SerializeField] private Color _nightEquatorColor;
        [SerializeField] private Color _dayLightColor;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Calendar _calendar;
        [SerializeField] private Color _nightLightColor;
        [SerializeField] private Energy _energy;
        [SerializeField] private EnergyNewDayScreen _energyNewDayScreen;
        [SerializeField] private float _dayAmbientIntensity;
        [SerializeField] private float _nightAmbientIntensity;

        // [SerializeField] private BuyersCounter _buyersCounter;
        [SerializeField] private Light sceneLight;

        [SerializeField] private TMP_Text _timeText;

        [SerializeField] private ClientsCreator _clientsCreator;

        public float minExposure = 0.5f;
        public float maxExposure = 1.65f;
        private float timeOfDay;
        [SerializeField] private bool _isDay;
        [SerializeField] private bool _isNight;
        private bool _isStatisticDayOpened;
        [SerializeField] private bool _isOpen;

        public event Action DayOverCompleted;

        public event Action NewDayStarted;

        public event Action<bool> SetNightLighting;

        public bool IsDay => _isDay;

        private void OnEnable()
        {
            _openCloseRestaurant.OpenedChanged += SetOpenValue;
        }

        private void OnDisable()
        {
            _openCloseRestaurant.OpenedChanged -= SetOpenValue;
        }

        private void Start()
        {
            _isDay = true;
            _isNight = !_isDay;

            _isDay = PlayerPrefs.GetInt(IS_DAY_KEY, 1) == 1;
            // _isNight = PlayerPrefs.GetInt(IS_NIGHT_KEY, 0) == 1;
            _isNight = !_isDay;

            timeOfDay = PlayerPrefs.GetFloat(TIME_OF_DAY_KEY, 0f);

            if (timeOfDay == 0f)
                timeOfDay = (START_HOUR - 9f) / (END_HOUR - 9f);

            RenderSettings.skybox = skyboxMaterial;

            if (!_isDay && !_isStatisticDayOpened)
            {
                DayOverCompleted?.Invoke();
                SetNightTime();
                UpdateTimeText(START_HOUR, END_HOUR, timeOfDay);
            }
            else
            {
                UpdateTimeText(START_HOUR, END_HOUR, timeOfDay);
                UpdateSkyboxColor(dayColor, nightColor, timeOfDay, maxExposure, minExposure, _dayAmbientColor,
                    _nightAmbientColor, _dayEquatorColor, _nightEquatorColor, _dayLightColor, _nightLightColor,
                    _dayAmbientIntensity,
                    _nightAmbientIntensity);
            }
        }

        private void Update()
        {
            if (!_isOpen)
                return;

            if (!_isDay && !_isStatisticDayOpened)
            {
                return;
            }

            if (!_isDay && !_isNight)
                return;

            if (_isDay)
            {
                UpdateDayCycle(dayDuration, false, true);

                UpdateSkyboxColor(dayColor, nightColor, timeOfDay, maxExposure, minExposure, _dayAmbientColor,
                    _nightAmbientColor, _dayEquatorColor, _nightEquatorColor, _dayLightColor, _nightLightColor,
                    _dayAmbientIntensity,
                    _nightAmbientIntensity);

                UpdateTimeText(START_HOUR, END_HOUR, timeOfDay);
            }

            if (_isNight)
            {
                UpdateDayCycle(_nightDuration, true, false);

                UpdateSkyboxColor(nightColor, dayColor, timeOfDay, minExposure, maxExposure, _nightAmbientColor,
                    _dayAmbientColor, _nightEquatorColor, _dayEquatorColor, _nightLightColor, _dayLightColor,
                    _nightAmbientIntensity,
                    _dayAmbientIntensity);

                UpdateTimeText(END_HOUR, START_HOUR, timeOfDay);
            }
        }

        public void ResetDay()
        {
            _energy.IncreaseEnergy(5);
            _playerLevel.AddExp(50);
            timeOfDay = 0f;
            _isDay = true;
            _isNight = false;
            SetDayTime();
            _calendar.NextDay();
            _energyNewDayScreen.OpenScreen();
        }

        public void SetOpenValue(bool value)
        {
            _isOpen = value;
        }

        private void UpdateDayCycle(float duration, bool isDay, bool isNight)
        {
            timeOfDay += Time.deltaTime / duration;

            if (timeOfDay >= 1f)
            {
                if (isNight)
                {
                    _isDay = isDay;
                    DayOverCompleted?.Invoke();
                }
                else
                {
                    timeOfDay = 0;
                    _isDay = isDay;
                    _isNight = isNight;
                }
            }

            if (RenderSettings.skybox != null)
            {
                RenderSettings.skybox.SetFloat("_Rotation", timeOfDay * 360f);
            }
        }

        private void SetNightTime()
        {
            skyboxMaterial.SetColor("_Tint", nightColor);
            skyboxMaterial.SetFloat("_Exposure", minExposure);
            RenderSettings.ambientSkyColor = _nightAmbientColor * _nightAmbientIntensity;
            RenderSettings.ambientEquatorColor = _nightEquatorColor;
            sceneLight.color = _nightLightColor;
        }

        public void SetDayTime()
        {
            skyboxMaterial.SetColor("_Tint", dayColor);
            skyboxMaterial.SetFloat("_Exposure", maxExposure);
            RenderSettings.ambientSkyColor = _dayAmbientColor * _dayAmbientIntensity;
            RenderSettings.ambientEquatorColor = _dayEquatorColor;
            sceneLight.color = _dayLightColor;
            UpdateTimeText(START_HOUR, END_HOUR, 0);
        }

        private void UpdateSkyboxColor(Color currentTintColor, Color targetTintColor, float duration,
            float currentExposure, float targetExposure, Color startAmbientColor, Color endAmbientColor,
            Color startEquatorColor, Color endEquatorColor, Color startLightColor, Color endLightColor,
            float startAmbientIntensity, float endAmbientIntensity)
        {
            Color currentColor = Color.Lerp(currentTintColor, targetTintColor, duration);
            skyboxMaterial.SetColor("_Tint", currentColor);
            float exposure = Mathf.Lerp(currentExposure, targetExposure, duration);
            skyboxMaterial.SetFloat("_Exposure", exposure);

            float currentAmbientIntensity = Mathf.Lerp(startAmbientIntensity, endAmbientIntensity, duration);
            Debug.Log(currentAmbientIntensity);
            Color currentAmbientColor = Color.Lerp(startAmbientColor, endAmbientColor, duration);
            RenderSettings.ambientSkyColor = currentAmbientColor * currentAmbientIntensity;

            Color currentEquatorColor = Color.Lerp(startEquatorColor, endEquatorColor, duration);
            RenderSettings.ambientEquatorColor = currentEquatorColor;
            Color currentLightColor = Color.Lerp(startLightColor, endLightColor, duration);
            sceneLight.color = currentLightColor;
        }


        private void UpdateTimeText(float startHour, float endHour, float duration)
        {
            if (_timeText != null)
            {
                float currentHour = Mathf.Lerp(startHour, endHour, duration);

                int hour = Mathf.FloorToInt(currentHour);
                int minute = Mathf.FloorToInt((currentHour - hour) * 60f);
                _timeText.text = string.Format("{0:00}:{1:00}", hour, minute);

                if (hour >= 21 && minute >= 0)
                    _clientsCreator.SetNightTime(true);
                else
                    _clientsCreator.SetNightTime(false);

                if (hour >= 18 && minute >= 0)
                    SetNightLighting?.Invoke(true);
                else
                    SetNightLighting?.Invoke(false);
            }
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetInt(IS_DAY_KEY, _isDay ? 1 : 0);
            PlayerPrefs.SetFloat(TIME_OF_DAY_KEY, timeOfDay);
            PlayerPrefs.Save();
        }
    }
}