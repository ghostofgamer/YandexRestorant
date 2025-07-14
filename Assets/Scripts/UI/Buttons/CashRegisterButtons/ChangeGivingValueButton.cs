using I2.Loc;
using RestaurantContent.CashRegisterContent;
using SettingsContent.SoundContent;
using UnityEngine;

namespace UI.Buttons
{
    public class ChangeGivingValueButton : AbstractButton
    {
        [SerializeField] private int _valueCents;
        [SerializeField] private CashRegister _cashRegister;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayCoins();
            _cashRegister.ChangeGivingValue(_valueCents);
        }
    }
}