using System;
using ClientsContent;
using DayNightContent;
using SettingsContent.SoundContent;
using UI.Screens;
using UnityEngine;

namespace StatisticContent
{
    public class StatisticViewer : MonoBehaviour
    {
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private StatisticsScreen _statisticsScreen;
        [SerializeField] private GameObject _statButton;
        [SerializeField] private ClientsCounter _clientsCounter;

        private void OnEnable()
        {
            _dayNightCycle.DayOverCompleted += ShowStatButton;
            _clientsCounter.ClientsListEmpted += ShowStatButton;
        }

        private void OnDisable()
        {
            _dayNightCycle.DayOverCompleted -= ShowStatButton;
            _clientsCounter.ClientsListEmpted -= ShowStatButton;
        }

        public void ShowStatistics()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _statisticsScreen.OpenScreen();
            _statisticsScreen.ShowStatistic();
            _statButton.SetActive(false);
        }

        private void ShowStatButton()
        {
            if (_clientsCounter.Clients.Count > 0 || _dayNightCycle.IsDay)
                return;

            _statButton.SetActive(true);
        }
    }
}