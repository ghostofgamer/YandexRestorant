using System.Collections.Generic;
using Enums;
using RestaurantContent;
using RestaurantContent.MenuContent;
using UnityEngine;

namespace OrdersContent
{
    public class OrderCreator : MonoBehaviour
    {
        [SerializeField] private Restaurant restaurant;
        [SerializeField] private MenuCounter _menuCounter;

        private List<ItemType> _cachedBurgers;
        private List<ItemType> _cachedDrinks;
        private List<ItemType> _cachedExtras;

        [ContextMenu("CreateOrder")]
        public Order CreateOrder()
        {
            _cachedBurgers = _menuCounter.GetBurgers();
            _cachedDrinks = _menuCounter.GetDrinks();
            _cachedExtras = _menuCounter.GetExtras();

            ItemType burgerType = GetRandomItemType(_cachedBurgers, "бургеров");

            if (burgerType != ItemType.Empty)
            {
                // Debug.Log($"а закажука я {burgerType}");
            }

            ItemType drinkType = GetRandomItemType(_cachedDrinks, "попить");

            if (drinkType != ItemType.Empty)
            {
                // Debug.Log($"а закажука я {drinkType}");
            }

            ItemType extraType = GetRandomItemType(_cachedExtras, "допов");

            if (extraType != ItemType.Empty)
            {
                // Debug.Log($"а закажука я {extraType}");
            }

            Order order = new Order(burgerType, drinkType, extraType);
            
            return order;
        }

        private ItemType GetRandomItemType(List<ItemType> itemList, string itemName)
        {
            if (itemList.Count > 0)
            {
                int randomIndex = Random.Range(0, itemList.Count);
                return itemList[randomIndex];
            }
            else
            {
                // Debug.Log($"!В меню нету {itemName}");
                return ItemType.Empty;
            }
        }
    }
}