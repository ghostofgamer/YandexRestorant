using MirraGames.SDK;
using SaveContent;
using SettingsContent;
using UnityEngine;

namespace UI.Screens
{
    public class FirstLanguageScreen : AbstractScreen
    {
        private const string FirstLanguageKey = "IsFirstLanguageChange";
        
        [SerializeField] private LanguageChanger _languageChanger;
        
        private bool _isFirstTime;
        
        public override void OpenScreen()
        {
            _isFirstTime = StorageHelper.GetInt(FirstLanguageKey, 0) == 0;

            if (_isFirstTime)
            {
                base.OpenScreen();
                StorageHelper.SetInt(FirstLanguageKey, 1);
            }
            else
            {
                CloseScreen();
            }
            
            
            
            /*_isFirstTime = PlayerPrefs.GetInt("IsFirstLanguageChange", 0) == 0;

            if (_isFirstTime)
            {
                base.OpenScreen();
                PlayerPrefs.SetInt("IsFirstLanguageChange", 1);
            }
            else
            {
                CloseScreen();
            }*/
        }
    }
}