using DayNightContent;
using SettingsContent.SoundContent;
using StatisticContent;
using UI.Screens;
using UnityEngine;
using WorkerContent;

namespace UI.Buttons
{
    public class NewDayButton : AbstractButton
    {
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private StatisticsScreen _statisticsScreen;
        [SerializeField] private StatisticCounter _statisticCounter;
        [SerializeField] private Workers _workers;
    
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _dayNightCycle.ResetDay();
            _statisticCounter.ClearData();
            _workers.PaySalary();
            _statisticsScreen.CloseScreen();
        }
    }
}