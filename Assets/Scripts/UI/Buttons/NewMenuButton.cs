using NewMenuContent;
using UI.Screens.ShopContent;
using UnityEngine;

namespace UI.Buttons
{
    public class NewMenuButton : AbstractButton
    {
        [SerializeField] private ShopScreen _shop;
        [SerializeField] private NewMenuButtonFade _newMenuButtonFade;

        public override void OnClick()
        {
            _shop.OpenMenuScreen();
            _newMenuButtonFade.Disable();
        }
    }
}