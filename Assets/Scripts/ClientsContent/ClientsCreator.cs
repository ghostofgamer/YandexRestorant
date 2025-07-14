using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using OrdersContent;
using ParkingContent;
using RestaurantContent;
using RestaurantContent.CashRegisterContent;
using RestaurantContent.MenuContent;
using RestaurantContent.TableContent;
using SpawnContent;
using TutorialContent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ClientsContent
{
    public class ClientsCreator : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private ClientsSpawner _clientsSpawner;
        [SerializeField] private OrderCreator _orderCreator;
        [SerializeField] private QueueCashRegister _queueCashRegister;
        [SerializeField] private Restaurant _restaurant;
        [SerializeField] private TablesCounter _tablesCounter;
        [SerializeField] private Transform _exitPosition;
        [SerializeField] private CashRegister _cashRegister;
        [SerializeField] private MenuCounter _menuCounter;

        [SerializeField] private ClientCar _clientCar;
        [SerializeField] private ClientCar[] _clientCars;
        [SerializeField] private Transform _carSpawnPosition;
        [SerializeField] private Transform _carParkingPosition;
        [SerializeField] private Parking _parking;

        [SerializeField] private bool _isWork = true;
        [SerializeField] private float _minTimeSpawn;
        [SerializeField] private float _maxTimeSpawn;

        [SerializeField] private Transform _exitCarPosition;

        [SerializeField] private PriceOrderCounter _priceOrderCounter;
        [SerializeField] private OpenCloseRestaurant _openCloseRestaurant;
        [SerializeField] private ClientsCounter _clientsCounter;

        private float _elapsedTime;
        private float _nextSpawnTime = 0f;
        private bool _isNightTime;

        public event Action ClientCreated;

        private void Start()
        {
            if ((int)_tutorial.CurrentType <= (int)TutorialType.TakeFirstOrder)
                _nextSpawnTime = 1;
            else
                _nextSpawnTime = Random.Range(_minTimeSpawn, _maxTimeSpawn);
        }

        private void Update()
        {
            if (_isWork && _openCloseRestaurant.IsOpened && !_isNightTime)
            {
                _elapsedTime += Time.deltaTime;

                if (_elapsedTime >= _nextSpawnTime)
                {
                    _elapsedTime = 0;


                    _nextSpawnTime = Random.Range(_minTimeSpawn, _maxTimeSpawn);

                    CreateClients();
                }
            }
        }

        [ContextMenu("Create New Client")]
        public void CreateClients()
        {
            if (_menuCounter.MenuList.Count <= 0)
            {
                Debug.Log("Меню пустое");
                return;
            }

            if (_queueCashRegister.IsQueueFull())
            {
                Debug.Log("Очередь заполнена");
                return;
            }

            if (_tablesCounter.GetFreeTableCount() <= 0)
            {
                Debug.Log("Свободных столов нету");
                return;
            }

            StartCoroutine(Create());
        }

        private IEnumerator Create()
        {
            /*if (_parking.GetCountFreeParkingPositions() > 0)
            {
                float randomValue = 1;

                /*if ((int)_tutorial.CurrentType > (int)TutorialType.CleanTable)
                    randomValue = Random.Range(0f, 1f);#1#

                if (randomValue < 0.5f)
                {
                    int freePositionsQueueAmount = _queueCashRegister.GetFreeQueuePositions();
                    int amountTables = _tablesCounter.GetFreeTableCount();

                    if (amountTables > 1 && freePositionsQueueAmount > 1)
                    {
                        InitClientsCar(Random.Range(1, 3));
                    }
                    else
                    {
                        InitClientCar();
                    }
                }
                else
                {
                    InitClientWalking();
                }
            }
            else
            {
                InitClientWalking();
            }*/

            InitClientWalking();
            yield return null;
        }

        public void SetNightTime(bool value)
        {
            _isNightTime = value;
        }

        private void InitClientCar()
        {
            InitClientsCar(1);
        }

        private void InitClientsCar(int clientCount)
        {
            List<Client> _clients = new List<Client>();

            for (int i = 0; i < clientCount; i++)
            {
                Table table = _tablesCounter.GetAvailableTable();
                table.SetBusyValue(true);
                Client client = _clientsSpawner.SpawnRandomClient();
                client.Init(_orderCreator.CreateOrder(), _restaurant, table, _exitPosition, _cashRegister,
                    _queueCashRegister, _priceOrderCounter, _clientsCounter);
                ClientCreated?.Invoke();
                _queueCashRegister.AddClientQueue(client);
                _clients.Add(client);
            }

            ParkingSpace parkingSpace = _parking.GetFreeParkingPosition();
            int randomCarIndex = Random.Range(0, _clientCars.Length);

            ClientCar car = Instantiate(_clientCars[randomCarIndex], _carSpawnPosition.position, Quaternion.identity,
                transform);
            parkingSpace.BusyPlace(car);

            foreach (var client in _clients)
            {
                car.AddClient(client, parkingSpace, _exitCarPosition);
                client.SetCar(car);
            }

            car.GoToPosition(parkingSpace.transform.position);
        }

        private void InitClientWalking()
        {
            Table table = _tablesCounter.GetAvailableTable();
            table.SetBusyValue(true);
            Client client = _clientsSpawner.SpawnRandomClient();
            client.Init(_orderCreator.CreateOrder(), _restaurant, table, _exitPosition, _cashRegister,
                _queueCashRegister, _priceOrderCounter, _clientsCounter);
            ClientCreated?.Invoke();
            _queueCashRegister.AddClientToQueue(client);
        }
    }
}