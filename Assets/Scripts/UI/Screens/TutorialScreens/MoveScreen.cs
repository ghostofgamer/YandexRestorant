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
        [SerializeField] private GameObject _keyboardImage;
        [SerializeField] private GameObject _leftThumpImage;

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
            LanguageChange();
            _keyboardImage.SetActive(!Application.isMobilePlatform);
            _leftThumpImage.SetActive(Application.isMobilePlatform);
        }

        private void LanguageChange()
        {
            if (Application.isMobilePlatform)
                _title.text =
                    $"{LocalizationManager.GetTermTranslation("Use")} <color=yellow>{LocalizationManager.GetTermTranslation("left thumb")}</color> {LocalizationManager.GetTermTranslation("to move")}";
            else
                _title.text =
                    $"{LocalizationManager.GetTermTranslation("Use")} <color=yellow> WASD </color> {LocalizationManager.GetTermTranslation("to move")}";
        }
    }
}