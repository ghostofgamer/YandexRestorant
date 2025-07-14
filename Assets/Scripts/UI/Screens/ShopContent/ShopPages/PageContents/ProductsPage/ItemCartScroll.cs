using System.Collections.Generic;
using DeliveryContent;
using Enums;
using PlayerContent.LevelContent;
using SettingsContent;
using SettingsContent.SoundContent;
using TMPro;
using TutorialContent;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class ItemCartScroll : MonoBehaviour
    {
        [SerializeField] private ItemCart _prefabItemCart;
        [SerializeField] private Transform _container;
        [SerializeField] private TMP_Text _totalPriceText;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Delivery _delivery;
        [SerializeField] private ShopScreen _shopScreen;
        [SerializeField] private Color _activeButtonColor;
        [SerializeField] private Color _notActiveButtonColor;
        [SerializeField] private Image _buyButtonImage;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private ShopTutorialChanger _shopTutorialChanger;
        [SerializeField] private LanguageChanger _languageChanger;

        private List<ItemCart> _items = new List<ItemCart>();
        private DollarValue _totalPrice;

        public void AddItemCart(ItemType itemType, int amount, DollarValue pricePerUnit, DollarValue totalPrice,
            string name)
        {
            ItemCart existingItem = _items.Find(item => item.ItemType == itemType);

            if (existingItem != null)
            {
                int newAmount = existingItem.CurrentAmount + amount;

                if (newAmount >= 10)
                    return;

                int totalCents = pricePerUnit.ToTotalCents(pricePerUnit) * newAmount;
                DollarValue newTotalPrice = new DollarValue(1, 1);
                newTotalPrice = pricePerUnit.FromTotalCents(totalCents);
                existingItem.UpdateAmount(newAmount, newTotalPrice);
                ShowTotalPrice();
            }
            else
            {
                ItemCart newItem = Instantiate(_prefabItemCart, _container);
                newItem.Init(itemType, amount, pricePerUnit, totalPrice, name, this, _languageChanger);
                _items.Add(newItem);
                ShowTotalPrice();
            }
        }

        public void UpdateItemCartInfo(ItemCart itemCart)
        {
            ItemCart existingItem = _items.Find(item => item.ItemType == itemCart.ItemType);

            if (existingItem != null)
            {
                int totalCents = existingItem.PricePerUnit.ToTotalCents(existingItem.PricePerUnit) *
                                 existingItem.CurrentAmount;
                DollarValue newTotalPrice = new DollarValue(0, 0);
                newTotalPrice = existingItem.PricePerUnit.FromTotalCents(totalCents);
                existingItem.UpdateAmount(existingItem.CurrentAmount, newTotalPrice);
            }
            else
            {
                Debug.Log("Данный предмет нуль");
            }

            ShowTotalPrice();
        }

        public void ShowTotalPrice()
        {
            int totalCents = 0;

            foreach (var item in _items)
            {
                totalCents += item.TotalPrice.ToTotalCents(item.TotalPrice);
            }

            DollarValue totalValue = new DollarValue(0, 0);
            totalValue = totalValue.FromTotalCents(totalCents);
            _totalPrice = totalValue;
            _totalPriceText.text = _totalPrice.ToString();

            _buyButtonImage.color = _wallet.DollarValue.ToTotalCents() >= _totalPrice.ToTotalCents()
                ? _activeButtonColor
                : _notActiveButtonColor;
        }

        public void PayItems()
        {
            SoundPlayer.Instance.PlayButtonClick();

            if (_items.Count <= 0)
                return;

            if (_wallet.ToTotalCents(_wallet.DollarValue) < _totalPrice.ToTotalCents(_totalPrice))
            {
                SoundPlayer.Instance.PlayError();
                Debug.Log("у тебя мало денег ");
                return;
            }

            int amountItems = 0;

            foreach (var item in _items)
            {
                amountItems += item.CurrentAmount;
            }

            if (_tutorial.CurrentType == TutorialType.OrderBurgerPatties)
            {
                _shopTutorialChanger.ResetProducts();
                _tutorial.SetCurrentTutorialStage(TutorialType.OrderBurgerPatties);
            }


            _playerLevel.AddExp(amountItems * 5);
            SoundPlayer.Instance.PlayPayment();
            _wallet.Subtract(_totalPrice);
            _delivery.AddItemsCart(_items);
            ClearItems();
            _shopScreen.CloseScreen();
            Debug.Log("тебе хватает денег ");
        }

        public void ClearItems()
        {
            foreach (var item in _items)
                Destroy(item.gameObject);

            _items.Clear();

            _totalPrice = new DollarValue(0, 0);
            _totalPriceText.text = _totalPrice.ToString();
        }

        public void DeleteItem(ItemCart itemCart)
        {
            if (_items.Contains(itemCart))
            {
                _items.Remove(itemCart);
                Destroy(itemCart.gameObject);
                ShowTotalPrice();
            }
        }
    }
}