using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using RestaurantContent;
using RestaurantContent.TrayContent;
using UnityEngine;

namespace KitchenEquipmentContent.AssemblyTables.CoffeeTableContent
{
    public class CoffeeCounter : MonoBehaviour
    {
        [SerializeField] private Restaurant _restaurant;
        
        private List<Item> _coffeeItems = new List<Item>();

        public event Action<int> CoffeeItemsValueChanged;

        public void AddCoffee(Item item)
        {
            _coffeeItems.Add(item);
            CoffeeItemsValueChanged?.Invoke(_coffeeItems.Count);
        }

        public void RemoveCoffee(Item item)
        {
            _coffeeItems.Remove(item);
            CoffeeItemsValueChanged?.Invoke(_coffeeItems.Count);
        }
        
        public void CheckWaitNeedCoffee(ItemType itemType)
        {
            if (TryFindNeedBurger(itemType, out Item coffee))
            {
                Sequence sequence = DOTween.Sequence();

                if (_restaurant.TryGetTrayDrinkOrder(itemType, out Tray tray))
                {
                    _restaurant.SetDrinkOrder(tray, coffee);
                    Transform position = tray.GetFirstAvailablePosition();

                    sequence.Append(coffee.transform.DOMove(position.position, 1f)
                        .SetEase(Ease.InOutQuad));

                    coffee.transform.SetParent(position);

                    sequence.Join(coffee.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => tray.TryCompletedOrder());
                }
            }
            else
            {
                // Debug.Log("FALSE Автоматическое Коффе");
            }
        }

        public bool TryFindNeedBurger(ItemType itemType, out Item burger)
        {
            burger = _coffeeItems.FirstOrDefault(b => b.ItemType == itemType);
            return burger != null;
        }
    }
}