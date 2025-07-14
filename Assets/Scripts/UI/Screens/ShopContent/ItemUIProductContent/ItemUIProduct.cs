using System;
using Enums;
using I2.Loc;
using ItemContent;
using SettingsContent;
using SettingsContent.SoundContent;
using SoContent;
using TMPro;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace UI.Screens.ShopContent.ItemUIProductContent
{
    public class ItemUIProduct : MonoBehaviour
    {
        [SerializeField] private IngredientsConfig _ingredientsConfig;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private int _minLevelToUnlock;
        [SerializeField] private GameObject _lockContent;
        [SerializeField] private GameObject _unlockContent;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameItemUIProduct;
        [SerializeField] private TMP_Text _levelRequiredText;
        [SerializeField] private ProductsScrollContent _productsScrollContent;
        [SerializeField] private LanguageChanger _languageChanger;

        private Ingredient _ingredient;
        
        public event Action<int, DollarValue> AmountChanged;

        public DollarValue PricePerUnit { get; private set; }

        public int AmountProduct { get; private set; } = 1;

        public DollarValue TotalPrice { get; private set; }

        public string Name { get; private set; }

        public ItemType ItemType => _itemType;

        private void Awake()
        {
            _ingredient = _ingredientsConfig.GetIngredient(_itemType);
        }

        private void OnEnable()
        {
            _languageChanger.LanguageChanged += ChangeLocalization;
            ChangeLocalization();
        }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= ChangeLocalization;
        }

        private void Start()
        {
            _ingredient = _ingredientsConfig.GetIngredient(_itemType);
            PricePerUnit = new DollarValue(_ingredient.dollarsPrice, _ingredient.centsPrice);
            _icon.sprite = _ingredient.shopItemSprite;
            // Name = _ingredient.name;
            Name = _ingredient.term;
            Debug.Log("ItemTERM " + _ingredient.term);
            _nameItemUIProduct.text = LocalizationManager.GetTermTranslation(_ingredient.term);
            // _nameItemUIProduct.text = Name;
            TotalPrice = PricePerUnit;
            AmountChanged?.Invoke(AmountProduct, PricePerUnit);
            ChangeLocalization();
        }

        private void ChangeLocalization()
        {
            _nameItemUIProduct.text = LocalizationManager.GetTermTranslation(_ingredient.term);
            Debug.Log("ItemTERM " + _ingredient.term);
            _levelRequiredText.text =
                $"{LocalizationManager.GetTermTranslation("Level to unlock")} {_minLevelToUnlock}";
        }

        public void IncreaseAmount()
        {
            SoundPlayer.Instance.PlayButtonClick();

            if (AmountProduct >= 9)
                return;

            AmountProduct++;
            ChangeTotalPrice();
            AmountChanged?.Invoke(AmountProduct, TotalPrice);
        }

        public void DecreaseAmount()
        {
            SoundPlayer.Instance.PlayButtonClick();

            if (AmountProduct > 1)
            {
                AmountProduct--;
                ChangeTotalPrice();
                AmountChanged?.Invoke(AmountProduct, TotalPrice);
            }
        }

        public void CheckUnlocked(int playerLevel)
        {
            _lockContent.SetActive(playerLevel < _minLevelToUnlock);
            _unlockContent.SetActive(playerLevel >= _minLevelToUnlock);
        }

        public void AddItemToCart()
        {
            _productsScrollContent.AddItem(this);
        }

        private void ChangeTotalPrice()
        {
            int totalCents = PricePerUnit.ToTotalCents(PricePerUnit) * AmountProduct;
            TotalPrice = PricePerUnit.FromTotalCents(totalCents);
        }
    }
}