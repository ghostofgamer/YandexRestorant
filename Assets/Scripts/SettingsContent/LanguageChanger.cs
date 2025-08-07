using System;
using System.Collections.Generic;
using I2.Loc;
using MirraGames.SDK;
using MirraGames.SDK.Common;
using UnityEngine;

namespace SettingsContent
{
    public class LanguageChanger : MonoBehaviour
    {
        public event Action LanguageChanged;

        private List<string> _languages = new List<string>
        {
            "English", "Spanish", "French", "German", "Italian", "Russian", "Japanese", "Turkish", "Polish",
            "Portuguese", "Indonesian"
        };

        private int currentIndex = 0;

        public void SetStartLanguage()
        {
            LanguageType languageType = MirraSDK.Language.Current;
            string currentLanguage = languageType.ToString();

            currentIndex = _languages.IndexOf(currentLanguage);

            if (PlayerPrefs.HasKey("LanguageIndex"))
            {
                currentIndex = PlayerPrefs.GetInt("LanguageIndex");
                LocalizationManager.CurrentLanguage = _languages[currentIndex];
                LanguageChanged?.Invoke();
            }
            else
            {
                if (currentIndex != -1 && LocalizationManager.HasLanguage(currentLanguage))
                {
                    LocalizationManager.CurrentLanguage = currentLanguage;
                    LanguageChanged?.Invoke();
                }
                else
                {
                    currentIndex = 0;
                    LocalizationManager.CurrentLanguage = _languages[currentIndex];
                    LanguageChanged?.Invoke();
                }
            }
        }

        public void PrevLanguage()
        {
            currentIndex = (currentIndex - 1 + _languages.Count) % _languages.Count;
            UpdateLanguageText();
            SaveIndex();
            LanguageChanged?.Invoke();
        }

        public void NextLanguage()
        {
            currentIndex = (currentIndex + 1) % _languages.Count;
            UpdateLanguageText();
            SaveIndex();
            LanguageChanged?.Invoke();
        }

        private void UpdateLanguageText()
        {
            if (LocalizationManager.HasLanguage(_languages[currentIndex]))
                LocalizationManager.CurrentLanguage = _languages[currentIndex];

            LanguageChanged?.Invoke();
        }

        private void SaveIndex()
        {
            PlayerPrefs.SetInt("LanguageIndex", currentIndex);
            PlayerPrefs.Save();
        }
    }
}