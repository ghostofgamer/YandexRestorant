using System;
using TMPro;
using UnityEngine;
using WalletContent;

namespace UI.Screens.ShopContent.ItemUIProductContent
{
    public class ItemUIProductViewer : MonoBehaviour
    {
        [SerializeField] private ItemUIProduct _itemUIProduct;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private TMP_Text _priceText;

        private void OnEnable()
        {
            _itemUIProduct.AmountChanged += UpdateUI;
        }

        private void OnDisable()
        {
            _itemUIProduct.AmountChanged -= UpdateUI;
        }

        /*private void Start()
        {
            DollarValue dollar = new DollarValue(1, 50);
            _priceText.text = dollar.ToString();
        }*/

        private void UpdateUI(int amountValue, DollarValue totalPrice)
        {
            _amountText.text = amountValue.ToString();
            _priceText.text = totalPrice.ToString();
        }
    }
}