using I2.Loc;
using SettingsContent;
using TMPro;
using UnityEngine;

namespace UI.Screens.TutorialScreens
{
    public class LookAroundScreen : AbstractScreen
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private LanguageChanger _languageChanger;
        [SerializeField] private GameObject _mouseImage;
        [SerializeField] private GameObject _rightThumpImage;

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
            _mouseImage.SetActive(!Application.isMobilePlatform);
            _rightThumpImage.SetActive(Application.isMobilePlatform);
        }

        private void LanguageChange()
        {
            if (Application.isMobilePlatform)
                _title.text =
                    $"{LocalizationManager.GetTermTranslation("Use")} <color=yellow>{LocalizationManager.GetTermTranslation("right thumb")}</color> {LocalizationManager.GetTermTranslation("look around")}";
            else
                _title.text =
                    $"{LocalizationManager.GetTermTranslation("Use")} <color=yellow>{LocalizationManager.GetTermTranslation("Mouse")}</color> {LocalizationManager.GetTermTranslation("look around")}";
        }
    }
}