using ADSContent;
using DayNightContent;
using Io.AppMetrica;
using SettingsContent.SoundContent;
using StatisticContent;
using UI.Screens;
using UnityEngine;
using WalletContent;
using WorkerContent;

namespace UI.Buttons
{
    public class DayAdOverButton : AbstractButton
    {
        [SerializeField] private ADS _ads;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private StatisticsScreen _statisticsScreen;
        [SerializeField] private StatisticCounter _statisticCounter;
        [SerializeField] private Workers _workers;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            _ads.ShowRewarded(() =>
            {
                AppMetrica.ReportEvent("RewardAD", "{\"" + "ExperienceOverDay" + "\":null}");
                _wallet.Add(new DollarValue(25, 0));
                _dayNightCycle.ResetDay();
                _statisticCounter.ClearData();
                _workers.PaySalary();
                _statisticsScreen.CloseScreen();
            });
        }
    }
}