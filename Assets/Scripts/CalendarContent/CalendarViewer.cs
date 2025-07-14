using I2.Loc;
using SettingsContent;
using TMPro;
using UnityEngine;

namespace CalendarContent
{
    public class CalendarViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _dayText;
        [SerializeField] private Calendar _calendar;
        [SerializeField] private LanguageChanger _languageChanger;
        
        private void OnEnable()
        {
            _calendar.DayChanged += Show;
            _languageChanger.LanguageChanged += Show;
        }

        private void OnDisable()
        {
            _calendar.DayChanged -= Show;
            _languageChanger.LanguageChanged -= Show;
        }

        private void Show()
        {
            _dayText.text = $"{LocalizationManager.GetTermTranslation("DAY")} {_calendar.CurrentDay}";
        }
    }
}