using System;
using System.Collections.Generic;
using Enums;
using SettingsContent.SoundContent;
using SoContent;
using TutorialContent;
using UI.Screens.ShopContent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DeliveryContent
{
    public class Delivery : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private DeliveryConfig _deliveryConfig;
        [SerializeField] private DeliveryViewer _deliveryViewer;
        [SerializeField] private DeliverySaver _deliverySaver;
        [SerializeField] private Tutorial _tutorial;

        private List<ItemDeliveryInfo> _items = new List<ItemDeliveryInfo>();
        private bool _isSpawning = false;
        private Coroutine _coroutine;
        private float _remainingTimeForNextSpawn;
        private DateTime _exitTime;

        public event Action<int> AmountItemsDeliveriesChanged;

        public event Action<float> TimeChanged;

        public event Action<GameObject> SpawnCompleted;

        public float RemainingTime { get; private set; }

        public int AmountDeliveries { get; private set; }

        public List<ItemDeliveryInfo> CurrentItems => _items;

        public float RemainingTimeForNextSpawn => _remainingTimeForNextSpawn;

        private void Start()
        {
            _deliverySaver.LoadDeliveryData();
        }

        private void Update()
        {
            if (_isSpawning && _items.Count > 0)
            {
                RemainingTime -= Time.deltaTime;

                TimeChanged?.Invoke(RemainingTime);

                if (RemainingTime <= 0)
                {
                    SpawnItem();

                    TimeChanged?.Invoke(0);

                    if (_items.Count > 0)
                        StartSpawning();
                }
            }
        }

        public void SetItemsList(List<ItemDeliveryInfo> items)
        {
            _items = new List<ItemDeliveryInfo>();

            if (items.Count > 0)
            {
                _items = items;
            }

            UpdateAmountDeliveries();
        }

        public void Init(List<ItemDeliveryInfo> items, float remainingTime, DateTime exitTime)
        {
            SetItemsList(items);
            RemainingTime = remainingTime;
            _exitTime = exitTime;

            if (items.Count > 0)
            {
                CheckDeliveryProgress();
            }
            else
            {
                // Debug.Log("Посылок нету");
            }
        }

        public void CheckDeliveryProgress()
        {
            DateTime currentTime = DateTime.UtcNow;

            TimeSpan elapsedTime = currentTime - _exitTime;
            double secondsElapsed = elapsedTime.TotalSeconds;


            if (secondsElapsed < RemainingTime)
            {
                RemainingTime -= (float)secondsElapsed;
                _isSpawning = true;
            }
            else
            {
                secondsElapsed -= RemainingTime;
                SpawnItem();
                int deliveriesToSpawn = (int)(secondsElapsed / _deliveryConfig.MinValueTimer);

                double remainingSeconds;

                if (secondsElapsed >= _deliveryConfig.MinValueTimer)
                    remainingSeconds = secondsElapsed % _deliveryConfig.MinValueTimer;
                else
                    remainingSeconds = _deliveryConfig.MinValueTimer - secondsElapsed;

                int actualDeliveriesToSpawn = Math.Min(deliveriesToSpawn, CurrentItems.Count);

                for (int i = 0; i < actualDeliveriesToSpawn; i++)
                {
                    SpawnItem();
                }

                if (CurrentItems.Count > 0)
                {
                    RemainingTime = (float)remainingSeconds;
                    _isSpawning = true;
                }
            }
        }

        public void SpawnAllItems()
        {
            _isSpawning = false;

            while (_items.Count > 0)
            {
                var item = _items[0];
                GameObject prefab = _deliveryConfig.GetPrefabByItemType(item.ItemType);

                if (prefab != null)
                {
                    GameObject newBox = Instantiate(prefab, GetPosition().position, Quaternion.identity);
                    SpawnCompleted?.Invoke(newBox);
                }

                item.Amount--;

                UpdateAmountDeliveries();

                if (item.Amount <= 0)
                    _items.RemoveAt(0);
            }

            if (_tutorial.CurrentType == TutorialType.SkipDelivery)
            {
                _tutorial.SetCurrentTutorialStage(TutorialType.SkipDelivery);
            }

            SoundPlayer.Instance.PlayDostavka();
            RemainingTime = 0;
            TimeChanged?.Invoke(RemainingTime);
        }

        public void AddItemsCart(List<ItemCart> items)
        {
            foreach (var item in items)
                _items.Add(new ItemDeliveryInfo(item.ItemType, item.CurrentAmount));

            if (!_isSpawning)
                StartSpawning();

            UpdateAmountDeliveries();
            _deliverySaver.SaveDeliveryData();
        }

        private void StartSpawning()
        {
            _isSpawning = true;
            RemainingTime = _deliveryConfig.MinValueTimer;
            TimeChanged?.Invoke(RemainingTime);
        }

        private void SpawnItem()
        {
            _isSpawning = false;

            if (_items.Count > 0)
            {
                var item = _items[0];
                GameObject prefab = _deliveryConfig.GetPrefabByItemType(item.ItemType);

                if (prefab != null)
                {
                    GameObject newBox = Instantiate(prefab, GetPosition().position, Quaternion.identity);
                    SpawnCompleted?.Invoke(newBox);
                }

                item.Amount--;

                SoundPlayer.Instance.PlayDostavka();

                UpdateAmountDeliveries();

                if (_tutorial.CurrentType == TutorialType.SkipDelivery)
                    _tutorial.SetCurrentTutorialStage(TutorialType.SkipDelivery);

                if (item.Amount <= 0)
                    _items.RemoveAt(0);
            }
        }

        public void SpawnPrize(ItemType itemType, int value)
        {
            for (int i = 0; i < value; i++)
            {
                GameObject prefab = _deliveryConfig.GetPrefabByItemType(itemType);
                SoundPlayer.Instance.PlayDostavka();

                if (prefab != null)
                {
                    GameObject box = Instantiate(prefab, GetPosition().position, Quaternion.identity);
                    SpawnCompleted?.Invoke(box);
                }
            }
        }

        private void UpdateAmountDeliveries()
        {
            AmountDeliveries = 0;

            foreach (var item in _items)
                AmountDeliveries += item.Amount;

            AmountItemsDeliveriesChanged?.Invoke(AmountDeliveries);
        }

        private Transform GetPosition()
        {
            int index = Random.Range(0, _spawnPositions.Length);
            return _spawnPositions[index];
        }
    }
}