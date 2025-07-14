using System;
using UnityEngine;

namespace CalendarContent
{
    public class Calendar : MonoBehaviour
    {
        private const string CurrentDayString = "CurrentDay";

        private int _minDay = 1;

        public event Action DayChanged;

        public int CurrentDay { get; private set; }

        private void Start()
        {
            CurrentDay = PlayerPrefs.GetInt(CurrentDayString, _minDay);
            DayChanged?.Invoke();
        }

        public void NextDay()
        {
            CurrentDay++;
            PlayerPrefs.SetInt(CurrentDayString, CurrentDay);
            DayChanged?.Invoke();
        }
    }
}