using Enums;
using IAP;
using UnityEngine;

namespace UI.Buttons
{
    public class PurchaseInapButon : AbstractButton
    {
        [SerializeField] private Purchaser _purchaser;
        [SerializeField] private PurchaseType _purchaseType;
    
        public override void OnClick()
        {
            _purchaser.ClickPurchaser(_purchaseType);
        }
    }
}