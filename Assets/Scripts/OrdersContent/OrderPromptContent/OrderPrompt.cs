using Enums;
using SoContent;
using SoContent.AssemblyBurger;
using UnityEngine;
using UnityEngine.UI;

namespace OrdersContent.OrderPromptContent
{
    public class OrderPrompt : MonoBehaviour
    {
        [SerializeField] private ItemsConfig _itemsConfig;
        [SerializeField] private Image _burgerImage;
        [SerializeField] private Image _drinkImage;
        [SerializeField] private Image _extraImage;
        [SerializeField] private Image _burgerCompletedImage;
        [SerializeField] private Image _drinkCompletedImage;
        [SerializeField] private Image _extraCompletedImage;
        [SerializeField] private Image _orderCompletedImage;
        [SerializeField] private IngridientPromt _ingridientPromt;

        private Order _order;
        private Recipes _recipes;

        public void InitOrder(Order order)
        {
            _order = order;
            CheckOrdersProgress();
            InitImages();
            SubscribeToOrderEvents();
        }

        private void InitImages()
        {
            SetImage(_burgerImage, _burgerCompletedImage, _order.BurgerItemOrder);
            SetImage(_drinkImage, _drinkCompletedImage, _order.DrinkItemOrder);
            SetImage(_extraImage, _extraCompletedImage, _order.ExtraItemOrder);
            _ingridientPromt.SetIngredients(_order);
        }

        private void SubscribeToOrderEvents()
        {
            if (_order != null)
                _order.ChangeOrder += OnOrderChanged;
        }

        private void UnsubscribeFromOrderEvents()
        {
            if (_order != null)
                _order.ChangeOrder -= CheckOrdersProgress;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            UnsubscribeFromOrderEvents();
            ResetCompletedImages();
            gameObject.SetActive(false);
        }

        private void SetImage(Image image, Image blackCompletedImage, ItemType itemType)
        {
            if (itemType != ItemType.Empty)
            {
                image.gameObject.SetActive(true);
                image.sprite = _itemsConfig.GetItemConfig(itemType).SpriteNotBackground;
                blackCompletedImage.sprite = image.sprite;
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }

        private void OnOrderChanged()
        {
            CheckOrdersProgress();
        }

        private void CheckOrdersProgress()
        {
            if (_order == null)
                return;

            _burgerCompletedImage.gameObject.SetActive(_order.IsBurgerCompleted);
            _drinkCompletedImage.gameObject.SetActive(_order.IsDrinkCompleted);
            _extraCompletedImage.gameObject.SetActive(_order.IsExtraCompleted);
            _orderCompletedImage.gameObject.SetActive(_order.IsOrderCompleted());
        }

        private void ResetCompletedImages()
        {
            _burgerCompletedImage.gameObject.SetActive(false);
            _drinkCompletedImage.gameObject.SetActive(false);
            _extraCompletedImage.gameObject.SetActive(false);
            _orderCompletedImage.gameObject.SetActive(false);
        }
    }
}