using System;
using ClientsContent;
using OrdersContent;
using PlayerContent.LevelContent;
using UnityEngine;
using WalletContent;

namespace StatisticContent
{
    public class StatisticCounter : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private ClientsCreator _clientsCreator;
        [SerializeField] private OrdersCounter _ordersCounter;

        public int Experience { get; private set; }
        public int Levels { get; private set; }
        public int Income { get; private set; }
        public int Expenses { get; private set; }
        public int TotalClients { get; private set; }
        public int TotalOrders { get; private set; }
        public int CompletedOrders { get; private set; }

        private void OnEnable()
        {
            _playerLevel.ExpAdded += AddExpDay;
            _playerLevel.LevelAdded += AddLevel;
            _wallet.IncomeChanged += AddIncome;
            _wallet.ExpensesChanged += AddExpenses;
            _clientsCreator.ClientCreated += AddClient;
            _ordersCounter.OrderAdded += AddOrder;
            _ordersCounter.OrderCompleted += AddCompletedOrder;
        }

        private void OnDisable()
        {
            _playerLevel.ExpAdded -= AddExpDay;
            _playerLevel.LevelAdded -= AddLevel;
            _wallet.IncomeChanged -= AddIncome;
            _wallet.ExpensesChanged -= AddExpenses;
            _clientsCreator.ClientCreated -= AddClient;
            _ordersCounter.OrderAdded -= AddOrder;
            _ordersCounter.OrderCompleted -= AddCompletedOrder;
        }

        private void Start()
        {
            LoadData();
        }

        private void AddExpDay(int exp)
        {
            if (exp <= 0)
                return;

            Experience += exp;
        }

        private void AddLevel() => Levels++;

        private void AddIncome(int incomeTotalCents)
        {
            Income += incomeTotalCents;
            SaveData();
        }

        private void AddExpenses(int expensesTotalCents)
        {
            Expenses += expensesTotalCents;
            SaveData();
        }

        private void AddClient()
        {
            TotalClients++;
            SaveData();
        }

        private void AddOrder()
        {
            TotalOrders++;
            SaveData();
        }

        private void AddCompletedOrder()
        {
            CompletedOrders++;
            SaveData();
        }


        public void SaveData()
        {
            PlayerPrefs.SetInt("Experience", Experience);
            PlayerPrefs.SetInt("Levels", Levels);
            PlayerPrefs.SetInt("Income", Income);
            PlayerPrefs.SetInt("Expenses", Expenses);
            PlayerPrefs.SetInt("TotalClients", TotalClients);
            PlayerPrefs.SetInt("TotalOrders", TotalOrders);
            PlayerPrefs.SetInt("CompletedOrders", CompletedOrders);

            PlayerPrefs.Save();
        }
        
        public void LoadData()
        {
            Experience = PlayerPrefs.GetInt("Experience", 0);
            Levels = PlayerPrefs.GetInt("Levels", 0);
            Income = PlayerPrefs.GetInt("Income", 0);
            Expenses = PlayerPrefs.GetInt("Expenses", 0);
            TotalClients = PlayerPrefs.GetInt("TotalClients", 0);
            TotalOrders = PlayerPrefs.GetInt("TotalOrders", 0);
            CompletedOrders = PlayerPrefs.GetInt("CompletedOrders", 0);
        }
        
        public void ClearData()
        {
            PlayerPrefs.DeleteKey("Experience");
            PlayerPrefs.DeleteKey("Levels");
            PlayerPrefs.DeleteKey("Income");
            PlayerPrefs.DeleteKey("Expenses");
            PlayerPrefs.DeleteKey("TotalClients");
            PlayerPrefs.DeleteKey("TotalOrders");
            PlayerPrefs.DeleteKey("CompletedOrders");

            Experience = 0;
            Levels = 0;
            Income = 0;
            Expenses = 0;
            TotalClients = 0;
            TotalOrders = 0;
            CompletedOrders = 0;
            // SaveData();
        }
    }
}