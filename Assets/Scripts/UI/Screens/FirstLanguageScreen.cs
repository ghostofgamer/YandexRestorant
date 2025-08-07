using SettingsContent;
using UnityEngine;

namespace UI.Screens
{
    public class FirstLanguageScreen : AbstractScreen
    {
        [SerializeField] private LanguageChanger _languageChanger;
        
        private bool _isFirstTime;

        public override void OpenScreen()
        {
            _isFirstTime = PlayerPrefs.GetInt("IsFirstLanguageChange", 0) == 0;

            if (_isFirstTime)
            {
                base.OpenScreen();
                PlayerPrefs.SetInt("IsFirstLanguageChange", 1);
            }
            else
            {
                CloseScreen();
            }
        }
    }
}