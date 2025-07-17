using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class PlayGameButton : AbstractButton
    {
        [SerializeField] private GameObject _loadScreen;
        [SerializeField] private FirstLanguageScreen _firstLanguageScreen;
        
        public override void OnClick()
        {
            _firstLanguageScreen.OpenScreen();
            _loadScreen.SetActive(false);
        }
    }
}