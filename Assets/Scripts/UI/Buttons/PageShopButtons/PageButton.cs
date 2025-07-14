using SettingsContent.SoundContent;
using UI.Screens;
using UI.Screens.ShopContent.ShopPages;
using UnityEngine;

namespace UI.Buttons.PageShopButtons
{
    public class PageButton : PageShopButton
    {
        [SerializeField] private ShopPage _shopPage;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _shopPage.Open(Index);
        }
    }
}