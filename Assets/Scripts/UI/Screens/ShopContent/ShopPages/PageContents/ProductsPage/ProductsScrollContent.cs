using PlayerContent.LevelContent;
using TutorialContent;
using UI.Screens.ShopContent.ItemUIProductContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class ProductsScrollContent : PageScrollContent
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private ItemCartScroll _itemCartScroll;
        [SerializeField] private ItemUIProduct[] _products;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private ShopTutorialChanger _shopTutorialChanger;

        private void OnEnable()
        {
            if (_scrollRect != null)
                _scrollRect.normalizedPosition = new Vector2(0, 1);

            _playerLevel.LevelChanged += UpdateRequiredLevelProducts;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= UpdateRequiredLevelProducts;
        }

        public override void Init()
        {
            _itemCartScroll.ClearItems();

            UpdateRequiredLevelProducts(_playerLevel.CurrentLevel);
            Debug.Log("GasmeObj " + gameObject.name);
        }

        public void AddItem(ItemUIProduct itemUIProduct)
        {
            _itemCartScroll.AddItemCart(
                itemUIProduct.ItemType,
                itemUIProduct.AmountProduct,
                itemUIProduct.PricePerUnit,
                itemUIProduct.TotalPrice,
                itemUIProduct.Name);
            
            _shopTutorialChanger.EnableBuyItems(itemUIProduct.ItemType);
        }

        private void UpdateRequiredLevelProducts(int level)
        {
            foreach (var product in _products)
                product.CheckUnlocked(level);
        }
    }
}