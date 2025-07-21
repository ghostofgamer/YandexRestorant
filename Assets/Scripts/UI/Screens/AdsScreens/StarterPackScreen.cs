using MirraGames.SDK;
using MirraGames.SDK.Common;
using TMPro;
using UnityEngine;

namespace UI.Screens.AdsScreens
{
    public class StarterPackScreen : AbstractScreen
    {
        [SerializeField] private TMP_Text _valuteText;

        public void Start()
        {
            ProductData productData = MirraSDK.Payments.GetProductData("StarterPack");
            _valuteText.text = $"{productData.Currency} {productData.PriceInteger}";
            Debug.Log(productData.PriceInteger);
                Debug.Log(productData.Currency);
        }
    }
}