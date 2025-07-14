using ADSContent;
using SettingsContent.SoundContent;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class CloseScreenButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _screen;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _screen.CloseScreen();
            // InterstitialActivator.Instance.ShowAd();
        }
    }
}
