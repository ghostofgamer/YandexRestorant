using MirraGames.SDK;
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
            _isFirstTime = MirraSDK.Data.GetInt(FirstLanguageKey, 0) == 0;

            if (_isFirstTime)
            {
                base.OpenScreen();
                MirraSDK.Data.SetInt(FirstLanguageKey, 1, true);
                MirraSDK.Data.Save();
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
        
        public void ClearSaveData()
        {
            // Сбрасываем флаг первого выбора языка
            MirraSDK.Data.SetInt(FirstLanguageKey, 0, true);
            MirraSDK.Data.Save();

            Debug.Log("First language screen data cleared. Will show again next time.");
        }
    }
}