using PromoCodeContent;
using SettingsContent.SoundContent;
using UnityEngine;

namespace UI.Buttons
{
    public class AcceptPromoCodeButton : AbstractButton
    {
        [SerializeField] private PromoCodeViewer _promoCodeViewer;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _promoCodeViewer.AcceptPromoCode();
        }
    }
}