using Enums;
using UI.Buttons.Tutor;
using UI.Screens.ShopContent;
using UI.Screens.ShopContent.ItemUIProductContent;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TutorialContent
{
    public class ShopTutorialChanger : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private ItemUIProduct[] _itemsItemUIProducts;
        [SerializeField] private ItemUIProduct _rawCutlet;
        [SerializeField] private Button _payItemsButton;
        [SerializeField] private Button _clearButton;
        [SerializeField] private GameObject _touchTutorBuyButton;
        [SerializeField] private GameObject _touchTutorRawCutlet;
        [SerializeField] private GameObject _touchTutorShopButton;
        [SerializeField] private Button AddItemToMenuSBurger;
        [FormerlySerializedAs("_touchAddItemToMenuButton")] [SerializeField] private GameObject _touchSliderToMenuButton;
        [SerializeField] private GameObject _touchAddItemToMenuButton;
        [SerializeField] private GameObject _touchCloseToMenuButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private ShopScreen _shopScreen;
        [SerializeField] private CloseTutorShopButton _closeTutorShop;

        public void SetValueShopButton(bool value)
        {
            // _shopScreen.CloseScreen();
            _touchCloseToMenuButton.SetActive(value);
            _closeButton.interactable = value;
            _closeTutorShop.enabled = value;
        }
        
        public void SetValueAddBurgerToMenuButton(bool value)
        {
            AddItemToMenuSBurger.interactable = value;
            _touchSliderToMenuButton.SetActive(!value);
            _touchAddItemToMenuButton.SetActive(value);
        }
        
        public void DeactivateProducts()
        {
            foreach (var itemsItemUI in _itemsItemUIProducts)
                itemsItemUI.gameObject.SetActive(false);
        }

        public void ActivateRawCutlet()
        {
            foreach (var itemsItemUI in _itemsItemUIProducts)
                itemsItemUI.gameObject.SetActive(false);

            _rawCutlet.gameObject.SetActive(true);
            _touchTutorRawCutlet.SetActive(true);
            _clearButton.interactable = false;
            _touchTutorShopButton.SetActive(false);
        }

        public void EnableBuyItems(ItemType itemType)
        {
            if (_tutorial.CurrentType == TutorialType.OrderBurgerPatties && itemType == ItemType.RawCutlet)
            {
                _rawCutlet.gameObject.SetActive(false);
                _payItemsButton.interactable = true;
                _touchTutorBuyButton.SetActive(true);
                _touchTutorRawCutlet.SetActive(false);
            }
        }

        public void ResetProducts()
        {
            foreach (var itemsItemUI in _itemsItemUIProducts)
                itemsItemUI.gameObject.SetActive(true);

            _touchTutorBuyButton.SetActive(false);
            _clearButton.interactable = true;
        }
    }
}