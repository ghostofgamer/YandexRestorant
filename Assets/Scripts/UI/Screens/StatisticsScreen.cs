using System;
using I2.Loc;
using StatisticContent;
using TMPro;
using UnityEngine;
using WalletContent;
using Calendar = CalendarContent.Calendar;

namespace UI.Screens
{
    public class StatisticsScreen : AbstractScreen
    {
        [SerializeField] private StatisticCounter _statisticCounter;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Calendar _calendar;
        [SerializeField] private TMP_Text _labelText;
        [SerializeField] private TMP_Text _totalClientsText;
        [SerializeField] private TMP_Text _totalOrdersText;
        [SerializeField] private TMP_Text _completedOrdersText;
        [SerializeField] private TMP_Text _experienceText;
        [SerializeField] private TMP_Text _levelsText;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private TMP_Text _expensesText;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private TMP_Text _balanceText;


        public void ShowStatistic()
        {
            _labelText.text =
                $"{LocalizationManager.GetTermTranslation("Report of the day")} ({LocalizationManager.GetTermTranslation("Day")} {_calendar.CurrentDay})";
            _totalClientsText.text =
                $"{LocalizationManager.GetTermTranslation("Total clients")}: {_statisticCounter.TotalClients}";
            _totalOrdersText.text =
                $"{LocalizationManager.GetTermTranslation("Total orders")}: {_statisticCounter.TotalOrders}";
            _completedOrdersText.text =
                $"{LocalizationManager.GetTermTranslation("Completed orders")}: {_statisticCounter.CompletedOrders}";
            _experienceText.text =
                $"{LocalizationManager.GetTermTranslation("Experience")}: {_statisticCounter.Experience}";
            _levelsText.text = $"{LocalizationManager.GetTermTranslation("Levels")}: +{_statisticCounter.Levels}";
            _incomeText.text =
                $"{LocalizationManager.GetTermTranslation("Income")}: {new DollarValue(0, 0).FromTotalCents(_statisticCounter.Income)}";
            _expensesText.text =
                $"{LocalizationManager.GetTermTranslation("Expenses")}: <color=#FF0000>-{new DollarValue(0, 0).FromTotalCents(_statisticCounter.Expenses)}</color>";

            int profitCents = _statisticCounter.Income - _statisticCounter.Expenses;
            bool isProfitNegative = profitCents < 0;
            int absProfitCents = Math.Abs(profitCents);
            string profitText = new DollarValue(0, 0).FromTotalCents(absProfitCents).ToString();
            string sign = isProfitNegative ? "-" : "+";
            string colorTag = isProfitNegative ? "<color=#FF0000>" : "<color=#00FF00>";
            string endColorTag = "</color>";

            _profitText.text = $"{LocalizationManager.GetTermTranslation("PROFIT")}: {colorTag}{sign}{profitText}{endColorTag}";
            _balanceText.text = $"{LocalizationManager.GetTermTranslation("BALANCE")}: {_wallet.DollarValue}";
        }
    }
}