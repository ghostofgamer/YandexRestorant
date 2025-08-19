using MysteryGiftContent;
using UnityEngine;

namespace UI.Screens
{
    public class MysteryBoxScreen : AbstractScreen
    {
        [SerializeField] private GameObject _input;
        [SerializeField] private MysteryGift _mysteryGift;

        public override void OpenScreen()
        {
            base.OpenScreen();

            if (Application.isMobilePlatform)
                _input.SetActive(false);
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
            _mysteryGift.DeactivateBox();

            if (Application.isMobilePlatform)
                _input.SetActive(true);
        }
    }
}