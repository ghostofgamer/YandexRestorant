using RestaurantContent.CashRegisterContent;
using SettingsContent.SoundContent;
using UnityEngine;

namespace UI.Buttons.CashRegisterButtons
{
    public class UndoChangeGivingValueButton : AbstractButton
    {
        [SerializeField] private CashRegister _cashRegister;
    
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _cashRegister.UndoLastChange();
        }
    }
}