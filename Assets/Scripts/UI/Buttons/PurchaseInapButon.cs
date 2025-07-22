using Enums;
using IAP;
using MirraGames.SDK;
using MirraGames.SDK.Common;
using TMPro;
using UnityEngine;

namespace UI.Buttons
{
    public class PurchaseInapButon : AbstractButton
    {
        [SerializeField] private Purchaser _purchaser;
        [SerializeField] private PurchaseType _purchaseType;
        [SerializeField] private TMP_Text _priceText;

        private void Start()
        {
            ProductData productData = MirraSDK.Payments.GetProductData(_purchaseType.ToString());
            _priceText.text = $"{productData.PriceInteger} {productData.Currency}";
        }

        public override void OnClick()
        {
            _purchaser.ClickPurchaser(_purchaseType);
        }
    }
}