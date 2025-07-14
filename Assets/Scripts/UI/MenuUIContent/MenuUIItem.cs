using System;
using I2.Loc;
using SoContent;
using TMPro;
using UnityEngine;
using WalletContent;

namespace UI.MenuUIContent
{
    public class MenuUIItem : AbstractUIMenuItem
    {
        private const string CurrentPriceKey = "CurrentPrice";

        [SerializeField] private TMP_Text _priceText;

        private void OnEnable()
        {
            int totalCents = PlayerPrefs.GetInt(CurrentPriceKey + ItemConfig.ItemType, 0);
            // _priceText.text = $"PRICE {new DollarValue(0, 0).FromTotalCents(totalCents)}";
            _priceText.text = $"{LocalizationManager.GetTermTranslation("Price")}:{new DollarValue(0, 0).FromTotalCents(totalCents)}";
        }

        public void RemoveItemToMenu()
        {
            _menuScrollContent.RemoveItem(ItemType);
        }

        public override void Init(ItemsConfig itemsConfig)
        {
            base.Init(itemsConfig);
            int totalCents = PlayerPrefs.GetInt(CurrentPriceKey + ItemConfig.ItemType, 0);
            _priceText.text = $"{LocalizationManager.GetTermTranslation("Price")}:{new DollarValue(0, 0).FromTotalCents(totalCents)}";
        }
    }
}