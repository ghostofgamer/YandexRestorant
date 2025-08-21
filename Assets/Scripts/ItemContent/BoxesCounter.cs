using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyBurgerContent;
using DeliveryContent;
using Enums;
using InputContent;
using LoadingSceneContent;
using RestaurantContent;
using SaveContent;
using SoContent;
using UnityEngine;

namespace ItemContent
{
    public class BoxesCounter : MonoBehaviour
    {
        [SerializeField] private Delivery _delivery;
        [SerializeField] private DeliveryConfig _deliveryConfig;
        [SerializeField] private BoxSaver _boxSaver;
        [SerializeField] private LoadingGame _loadingGame;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private AssemblyTable _assemblyTable;
        [SerializeField] private Trash _trash;

        private List<ItemBasket> _itemBaskets = new List<ItemBasket>();
        private List<ItemDrinkPackage> _itemDrinkPackages = new List<ItemDrinkPackage>();

        public List<ItemBasket> ItemBaskets => _itemBaskets;
        public List<ItemDrinkPackage> ItemDrinkPackages => _itemDrinkPackages;

        private Coroutine _coroutine;

        private void OnEnable()
        {
            _delivery.SpawnCompleted += AddBox;
            _playerInput.ThrowEvent += SaveBoxValue;
            _trash.BasketDeleted += SaveBoxValue;
            // _assemblyTable.IngredientsAdded += SaveBoxValue;

            // _loadingGame.MirraSDKInitialization += Initialize;
            // _delivery.SpawnAllCompleted += SaveBoxValue;
        }

        private void OnDisable()
        {
            _delivery.SpawnCompleted -= AddBox;
            _playerInput.ThrowEvent -= SaveBoxValue;
            _trash.BasketDeleted -= SaveBoxValue;
            // _assemblyTable.IngredientsAdded -= SaveBoxValue;

            // _loadingGame.MirraSDKInitialization -= Initialize;
            // _delivery.SpawnAllCompleted -= SaveBoxValue;
        }

        private void Start()
        {
            Load();
        }

        /*public void Initialize()
        {
            _boxSaver.LoadData();
            Load();
        }*/

        public void RemoveBox(GameObject box)
        {
            if (box.TryGetComponent(out ItemBasket itemBasket))
            {
                itemBasket.TransferProductsEnded -= SaveBoxValue;
                _itemBaskets.Remove(itemBasket);
            }

            if (box.TryGetComponent(out ItemDrinkPackage itemDrinkPackage))
            {
                itemDrinkPackage.FullnessChanged -= SaveBoxValue;
                _itemDrinkPackages.Remove(itemDrinkPackage);
            }

            Debug.Log("RemoveBox");
            _boxSaver.SaveData();
        }

        public void AddBox(GameObject box)
        {
            if (box.TryGetComponent(out ItemBasket itemBasket))
            {
                _itemBaskets.Add(itemBasket);
                itemBasket.TransferProductsEnded += SaveBoxValue;
            }

            if (box.TryGetComponent(out ItemDrinkPackage itemDrinkPackage))
            {
                _itemDrinkPackages.Add(itemDrinkPackage);
                itemDrinkPackage.FullnessChanged += SaveBoxValue;
            }

            Debug.Log("AddBox");
            SaveBoxValue();
            // _boxSaver.SaveData();
        }

        private IEnumerator Save()
        {
            yield return new WaitForSeconds(0.1f);

            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!SaveBoxValue!!!!!!!!!!!!!!!!!!!!!!!!");
            _boxSaver.SaveData();
        }

        private void SaveBoxValue()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Save());
        }

        public ItemBasket GetItemBasketByType(ItemType itemType)
        {
            return ItemBaskets.FirstOrDefault(item => item.ItemType == itemType);
        }

        private void Load()
        {
            List<BoxData> loadedBoxes = _boxSaver.LoadData();
            Debug.Log("loadedBoxes " + loadedBoxes.Count);

            foreach (BoxData boxData in loadedBoxes)
            {
                GameObject prefab = _deliveryConfig.GetPrefabByItemType((ItemType)boxData.itemType);

                if (prefab != null)
                {
                    GameObject box = Instantiate(prefab, boxData.position, Quaternion.identity, this.transform);
                    LoadBox(box, boxData);
                }
            }

            SaveBoxValue();
        }

        private void LoadBox(GameObject box, BoxData boxData)
        {
            if (box.TryGetComponent(out ItemBasket itemBasket))
            {
                _itemBaskets.Add(itemBasket);
                itemBasket.TransferProductsEnded += SaveBoxValue;

                if (boxData.additional)
                {
                    Debug.Log("Additional BOX " + boxData.itemType);
                    itemBasket.LoadItems(true, boxData.amount, boxData.additionalAmountItems);
                    Debug.Log("16");
                }
                else
                {
                    itemBasket.LoadItems(false, boxData.amount, boxData.additionalAmountItems);
                }
            }

            if (box.TryGetComponent(out ItemDrinkPackage itemDrinkPackage))
            {
                _itemDrinkPackages.Add(itemDrinkPackage);
                itemDrinkPackage.FullnessChanged += SaveBoxValue;
                itemDrinkPackage.SetFullness(boxData.amount);
            }
        }

        public void Clear()
        {
            _itemBaskets.Clear();
            _itemDrinkPackages.Clear();
        }
    }
}