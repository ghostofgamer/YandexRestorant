using SettingsContent.SoundContent;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons.SettingsScreenButton
{
    public class DiscordButton : AbstractButton
    {
        [SerializeField] private SettingsScreen _settingsScreen;

        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _settingsScreen.OpenDiscord();
        }
    }
}