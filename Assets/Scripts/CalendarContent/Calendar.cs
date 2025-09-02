using System;
using LoadingSceneContent;
using SaveContent;
using UnityEngine;

namespace CalendarContent
{
    public class Calendar : MonoBehaviour
    {
        private const string CurrentDayString = "CurrentDay";
        
        [SerializeField]private LoadingGame _loadingGame;
        
        private int _minDay = 1;

        public event Action DayChanged;

        public int CurrentDay { get; private set; }

        private void OnEnable()
        {
            _loadingGame.MirraSDKInitialization += Init; 
        }

        private void OnDisable()
        {
            _loadingGame.MirraSDKInitialization -= Init;
        }

        /*private void Start()
        {
            // CurrentDay = PlayerPrefs.GetInt(CurrentDayString, _minDay);
            CurrentDay = StorageHelper.GetInt(CurrentDayString, _minDay);
            DayChanged?.Invoke();
        }*/

        private void Init()
        {
            CurrentDay = StorageHelper.GetInt(CurrentDayString, _minDay);
            DayChanged?.Invoke();
        }

        public void NextDay()
        {
            CurrentDay++;
            // PlayerPrefs.SetInt(CurrentDayString, CurrentDay);
            StorageHelper.SetInt(CurrentDayString, CurrentDay);
            DayChanged?.Invoke();
        }
    }
}