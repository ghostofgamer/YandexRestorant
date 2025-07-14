using InputContent;
using UnityEngine;

namespace UI.Screens
{
    public class SettingsScreen : AbstractScreen
    {
        public void OpenDiscord()
        {
            Application.OpenURL("https://discord.gg/QvQSNvJZ");
        }

        public void OpenLifeFrameSite()
        {
            Application.OpenURL("http://lifeframestudios.tilda.ws");
        }
    }
}