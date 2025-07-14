using System;
using System.Collections.Generic;
using System.Linq;
using ClientsContent;
using Enums;
using KitchenEquipmentContent.AssemblyTables.CoffeeTableContent;
using KitchenEquipmentContent.AssemblyTables.SodaTableContent;
using KitchenEquipmentContent.FryerContent;
using PlayerContent.LevelContent;
using RestaurantContent;
using RestaurantContent.TrayContent;
using UnityEngine;

namespace OrdersContent
{
    public class OrdersCounter : MonoBehaviour
    {
        [SerializeField] private TrayCounter _trayCounter;
        [SerializeField] private List<Client> _activeOrderWaitClients;
        [SerializeField] private Restaurant _restaurant;
        [SerializeField] private BurgersCounter _burgersCounter;
        [SerializeField] private CoffeeCounter _coffeeCounter;
        [SerializeField] private SodaCounter _sodaCounter;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private DeepFryerItemCounter _deepFryerItemCounter;

        private const int MaxActiveOrders = 4;

        private List<Order> _currentOrders;
        private Queue<Order> _orderQueue;
        private Queue<Client> _clientQueue;

        public event Action<List<Order>> OrdersChanged;

        public event Action<int> UpdateOrders;

        public event Action OrderAdded;
        public event Action OrderCompleted;

        public List<Order> CurrentOrders => _currentOrders;


        private void Start()
        {
            _currentOrders = new List<Order>();
            _orderQueue = new Queue<Order>();
            _clientQueue = new Queue<Client>();
        }

        public void AddOrder(Order order, Client client)
        {
            if (_currentOrders.Count < MaxActiveOrders && GetValueFreeTrays())
            {
                _currentOrders.Add(order);
                _activeOrderWaitClients.Add(client);
                UpdateTrays(order);
                Debug.Log("Добавлен новый активный заказ: " + order.IndexTable);
                OrdersChanged?.Invoke(_currentOrders);
                // OrdersChanged?.Invoke(order);
                _burgersCounter.CheckWaitNeedBurgers(order.BurgerItemOrder);
                _coffeeCounter.CheckWaitNeedCoffee(order.DrinkItemOrder);
                _sodaCounter.CheckWaitNeedSoda(order.DrinkItemOrder);
                _deepFryerItemCounter.CheckWaitNeedBurgers(order.ExtraItemOrder);
                OrderAdded?.Invoke();
            }
            else
            {
                _orderQueue.Enqueue(order);
                _clientQueue.Enqueue(client);
                OrderAdded?.Invoke();
                Debug.Log("Добавлен новый заказ в очередь: " + order.IndexTable);
            }
        }

        public void CompleteOrder(Order order, Tray tray)
        {
            if (_currentOrders.Contains(order))
            {
                Debug.LogWarning(" нашли заказ ");

                _currentOrders.Remove(order);
                // OrderDeleted?.Invoke(order);
                Client client = _activeOrderWaitClients.FirstOrDefault(c => c.Order == order);

                if (client != null)
                {
                    Debug.Log(" client иди за заказом " + client);
                    _activeOrderWaitClients.Remove(client);
                    client.OrderCompleted(tray);
                    _playerLevel.AddExp(5);
                }
                else
                {
                    Debug.LogError(" Не анходит клиента  которого заказ! ");
                }

                Debug.Log("_currentOrders " + _currentOrders.Count);

                foreach (var currentOrder in _currentOrders)
                {
                    Debug.Log("CURRENT ORDER INDEX " + currentOrder.IndexTable + 1);
                }

                UpdateOrders?.Invoke(_currentOrders.Count);
                OrdersChanged?.Invoke(_currentOrders);
                OrderCompleted?.Invoke();
                Debug.Log("FFF ");
                // TryActivateOrder();
            }
            else
            {
                Debug.LogError("не находит нужный выполненный заказ");
            }
        }

        public void TryActivateOrder()
        {
            Debug.Log("_currentOrders.Count < MaxActiveOrders " + (_currentOrders.Count < MaxActiveOrders));

            while (_currentOrders.Count < MaxActiveOrders && _orderQueue.Count > 0)
            {
                if (_trayCounter.GetFreeTrayCount() <= 0)
                    return;

                Order nextOrder = _orderQueue.Dequeue();
                Client nextClient = _clientQueue.Dequeue();
                _currentOrders.Add(nextOrder);
                _activeOrderWaitClients.Add(nextClient);
                UpdateTrays(nextOrder);

                // OrdersChanged?.Invoke(nextOrder);
                OrdersChanged?.Invoke(_currentOrders);

                _burgersCounter.CheckWaitNeedBurgers(nextOrder.BurgerItemOrder);
                _coffeeCounter.CheckWaitNeedCoffee(nextOrder.DrinkItemOrder);
                _sodaCounter.CheckWaitNeedSoda(nextOrder.DrinkItemOrder);
                _deepFryerItemCounter.CheckWaitNeedBurgers(nextOrder.ExtraItemOrder);

                Debug.Log("Активирован новый заказ: " + nextOrder.IndexTable);
            }
        }

        public bool GetValueFreeTrays()
        {
            return _trayCounter.GetFreeTrayCount() > 0;
        }
        
        private void UpdateTrays(Order order)
        {
            int freeTraysCount = _trayCounter.GetFreeTrayCount();
            Debug.Log("Количество свободных подносов: " + freeTraysCount);

            if (freeTraysCount <= 0)
                return;

            Tray freeTray = _trayCounter.GetTray();

            if (freeTray != null)
            {
                Debug.Log("Поднос нашли ");
                freeTray.SetBusy(true);
                freeTray.SetOrder(order);
            }
            else
            {
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
                Debug.LogError("Поднос Null у тебя");
            }
        }

        public Order GetOrderByBurger(ItemType burgerType)
        {
            // return _currentOrders.FirstOrDefault(order => order.BurgerItemOrder == burgerType);
            return _currentOrders.FirstOrDefault(order =>
                order.BurgerItemOrder == burgerType && !order.IsBurgerCompleted);
        }

        public Order GetOrderByDrink(ItemType drinkItem)
        {
            // return _currentOrders.FirstOrDefault(order => order.BurgerItemOrder == burgerType);
            return _currentOrders.FirstOrDefault(order => order.DrinkItemOrder == drinkItem && !order.IsDrinkCompleted);
        }

        public Order GetOrderByExtra(ItemType extraItem)
        {
            // return _currentOrders.FirstOrDefault(order => order.BurgerItemOrder == burgerType);
            return _currentOrders.FirstOrDefault(order => order.ExtraItemOrder == extraItem && !order.IsExtraCompleted);
        }
    }
}