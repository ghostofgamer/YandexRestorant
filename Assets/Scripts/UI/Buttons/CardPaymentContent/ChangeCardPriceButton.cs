using RestaurantContent.CashRegisterContent;
using SettingsContent.SoundContent;
using UnityEngine;

namespace UI.Buttons.CardPaymentContent
{
    public class ChangeCardPriceButton : AbstractButton
    {
        [SerializeField] private CardPaymentAccountant _cardPaymentAccountant;
        [SerializeField] private int _value;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _cardPaymentAccountant.OnDigitButtonClick(_value);
        }
    }
}