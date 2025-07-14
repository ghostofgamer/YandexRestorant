using System;
using I2.Loc;
using SettingsContent;
using TMPro;
using UnityEngine;

namespace UI.Screens.TutorialScreens
{
    public class MoveScreen : AbstractScreen
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private LanguageChanger _languageChanger;
        
        private void OnEnable()
        {
            _languageChanger.LanguageChanged += LanguageChange;
        }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= LanguageChange;
        }
        
        private void Start()
        {
            // _title.text = "Use <color=yellow>left thumb</color> to move";
            LanguageChange();
        }
        
        private void  LanguageChange()
        {
            _title.text =
                $"{LocalizationManager.GetTermTranslation("Use")} <color=yellow>{LocalizationManager.GetTermTranslation("left thumb")}</color> {LocalizationManager.GetTermTranslation("to move")}";
        }
    }
}