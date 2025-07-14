using System.Collections.Generic;
using KitchenEquipmentContent.AssemblyTables.CoffeeTableContent;
using KitchenEquipmentContent.AssemblyTables.SodaTableContent;
using KitchenEquipmentContent.FryerContent;
using OrdersContent;
using TMPro;
using UnityEngine;

namespace RestaurantContent.TrayContent
{
    public class Tray : MonoBehaviour
    {
        [SerializeField] private GameObject _check;
        [SerializeField] private TMP_Text _indexTable;
        [SerializeField] private Transform[] _itemPositions;

        private Transform _defaultParent;
        private OrdersCounter _ordersCounter;
        private BurgersCounter _burgersCounter;
        private CoffeeCounter _coffeeCounter;
        private SodaCounter _sodaCounter;
        private DeepFryerItemCounter _deepFryerItemCounter;

        private List<Item> _items = new List<Item>();

        public Order Order { get; private set; }

        public bool IsBusy { get; private set; }

        public Transform[] ItemPositions => _itemPositions;

        public void Init(OrdersCounter ordersCounter, Transform defaultParent, BurgersCounter burgersCounter,
            CoffeeCounter coffeeCounter, SodaCounter sodaCounter,DeepFryerItemCounter deepFryerItemCounter)
        {
            _ordersCounter = ordersCounter;
            _defaultParent = defaultParent;
            _burgersCounter = burgersCounter;
            _coffeeCounter = coffeeCounter;
            _sodaCounter = sodaCounter;
            _deepFryerItemCounter = deepFryerItemCounter;
        }

        public void SetBusy(bool value)
        {
            IsBusy = value;
        }

        public void SetOrder(Order order)
        {
            Order = order;
            _check.SetActive(true);
            _indexTable.text = (order.IndexTable + 1).ToString();
            
            Debug.Log("SetOrder" + order.IndexTable + 1);
        }

        [ContextMenu("Completed")]
        public void Completed()
        {
            Debug.Log("Completed Tray");
            _ordersCounter.CompleteOrder(Order, this);
        }

        public void Clear()
        {
            SetBusy(false);
            Order = null;
            _check.SetActive(false);
        }

        public void DefaultReturn()
        {
            foreach (var item in _items)
                item.ReturnDefaultParent();

            _items.Clear();

            Debug.Log("Вернули в пул");
            Clear();
            transform.parent = _defaultParent;
        }

        public void SetActivity(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetBurger(Item item)
        {
            _items.Add(item);
            _burgersCounter.RemoveBurger(item);
            Order.SetBurgerCompleted(true);
        }

        public void SetDrink(Item item)
        {
            _items.Add(item);
            _coffeeCounter.RemoveCoffee(item);
            Order.SetDrinkCompleted(true);
        }

        public void SetSodaDrink(Item item)
        {
            _items.Add(item);
            _sodaCounter.RemoveSoda(item);
            Order.SetDrinkCompleted(true);
        }

        public void SetExtra(Item item)
        {
            _items.Add(item);
            _deepFryerItemCounter.RemoveItem(item);
            Order.SetExtraCompleted(true);
        }
        
        public void TryCompletedOrder()
        {
            if (Order.IsOrderCompleted())
            {
                Debug.Log("Заказ завершен");
                Completed();
            }
        }

        public Transform GetFirstAvailablePosition()
        {
            foreach (var position in _itemPositions)
            {
                if (position.childCount == 0)
                    return position;
            }

            return null;
        }
    }
}