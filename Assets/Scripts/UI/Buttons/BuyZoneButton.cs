using SettingsContent.SoundContent;
using UI.Screens.ShopContent;
using UnityEngine;

namespace UI.Buttons
{
    public class BuyZoneButton : AbstractButton
    {
        [SerializeField] private ZoneUIProduct _zoneUIProduct;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _zoneUIProduct.Buy();
        }
    }
}