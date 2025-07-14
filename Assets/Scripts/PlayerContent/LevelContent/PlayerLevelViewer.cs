using I2.Loc;
using SettingsContent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerContent.LevelContent
{
    public class PlayerLevelViewer : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Image _fillImage;
        [SerializeField] private TMP_Text _levelValueText;
        [SerializeField] private LanguageChanger _languageChanger;
        
        private int _currentLevel;
        
        private void OnEnable()
        {
            _playerLevel.LevelChanged += ShowLevelValue;
            _playerLevel.ExpChanged += ShowExperience;
            _languageChanger.LanguageChanged += ChangeLocalization;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= ShowLevelValue;
            _playerLevel.ExpChanged -= ShowExperience;
            _languageChanger.LanguageChanged -= ChangeLocalization;
        }

        private void ShowLevelValue(int levelValue)
        {
            _levelValueText.text = $"{LocalizationManager.GetTermTranslation("Lv.")} {levelValue}";
        }

        private void ChangeLocalization()
        {
            _levelValueText.text = $"{LocalizationManager.GetTermTranslation("Lv.")} {_playerLevel.CurrentLevel}";
        }
        
        private void ShowExperience(int currentValue, int maxValue)
        {
            _fillImage.fillAmount = (float)currentValue / maxValue;
        }
    }
}