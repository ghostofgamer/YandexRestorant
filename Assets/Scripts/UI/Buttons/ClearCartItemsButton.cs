using SettingsContent.SoundContent;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;

namespace UI.Buttons
{
    public class ClearCartItemsButton : AbstractButton
    {
        [SerializeField] private ItemCartScroll _itemCartScroll;

        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _itemCartScroll.ClearItems();
        }
    }
}