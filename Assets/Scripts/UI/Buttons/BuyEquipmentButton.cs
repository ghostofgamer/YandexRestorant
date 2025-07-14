using SettingsContent.SoundContent;
using UI.Screens;
using UI.Screens.EquipmentContent;
using UnityEngine;

namespace UI.Buttons
{
    public class BuyEquipmentButton : AbstractButton
    {
        [SerializeField] private EquipmentUIProduct _equipmentUIProduct;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _equipmentUIProduct.Buy();
        }
    }
}