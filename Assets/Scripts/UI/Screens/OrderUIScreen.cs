using Enums;
using I2.Loc;
using OrdersContent;
using SoContent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class OrderUIScreen : MonoBehaviour
    {
        [SerializeField] private ItemsConfig _itemsConfig;
        [SerializeField] private TMP_Text _tableIndexText;

        [SerializeField] private Image[] _images;

        public Order Order { get; private set; }

        public void Init(Order order)
        {
            gameObject.SetActive(true);
            Order = order;

            if (Order.BurgerItemOrder != ItemType.Empty)
            {
                ItemConfig burgerConfig = _itemsConfig.GetItemConfig(Order.BurgerItemOrder);
                SetSprite(burgerConfig.Sprite);
            }
            else
            {
                // Debug.Log("Бургер empty");
            }

            if (Order.DrinkItemOrder != ItemType.Empty)
            {
                ItemConfig drinkConfig = _itemsConfig.GetItemConfig(Order.DrinkItemOrder);
                SetSprite(drinkConfig.Sprite);
            }
            else
            {
                // Debug.Log("напиток empty");
            }

            if (Order.ExtraItemOrder != ItemType.Empty)
            {
                ItemConfig extraConfig = _itemsConfig.GetItemConfig(Order.ExtraItemOrder);
                SetSprite(extraConfig.Sprite);
            }
            else
            {
                // Debug.Log("Доп empty");
            }

            _tableIndexText.text = $"{LocalizationManager.GetTermTranslation("Table")}: {(Order.IndexTable + 1)}";
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);

            foreach (var image in _images)
                image.gameObject.SetActive(false);
        }

        private void SetSprite(Sprite sprite)
        {
            foreach (var image in _images)
            {
                if (!image.gameObject.activeSelf)
                {
                    image.gameObject.SetActive(true);
                    image.sprite = sprite;
                    return;
                }
            }
        }
    }
}