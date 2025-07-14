using SettingsContent.SoundContent;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class OpenScreenButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _screen;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _screen.OpenScreen();
        }
    }
}