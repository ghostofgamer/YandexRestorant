using System.Collections.Generic;
using System.Linq;
using DeliveryContent;
using Enums;
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

        private List<ItemBasket> _itemBaskets = new List<ItemBasket>();
        private List<ItemDrinkPackage> _itemDrinkPackages = new List<ItemDrinkPackage>();

        public List<ItemBasket> ItemBaskets => _itemBaskets;
        public List<ItemDrinkPackage> ItemDrinkPackages => _itemDrinkPackages;

        private void OnEnable()
        {
            _delivery.SpawnCompleted += AddBox;
        }

        private void OnDisable()
        {
            _delivery.SpawnCompleted -= AddBox;
        }

        private void Start()
        {
            Load();
        }

        public void RemoveBox(GameObject box)
        {
            if (box.TryGetComponent(out ItemBasket itemBasket))
                _itemBaskets.Remove(itemBasket);

            if (box.TryGetComponent(out ItemDrinkPackage itemDrinkPackage))
                _itemDrinkPackages.Remove(itemDrinkPackage);
            
            // _boxSaver.SaveData();
        }

        public void AddBox(GameObject box)
        {
            if (box.TryGetComponent(out ItemBasket itemBasket))
                _itemBaskets.Add(itemBasket);

            if (box.TryGetComponent(out ItemDrinkPackage itemDrinkPackage))
                _itemDrinkPackages.Add(itemDrinkPackage);
            
            // _boxSaver.SaveData();
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
                    GameObject box = Instantiate(prefab, boxData.position, Quaternion.identity);
                    LoadBox(box, boxData);
                }
            }
        }

        private void LoadBox(GameObject box, BoxData boxData)
        {
            if (box.TryGetComponent(out ItemBasket itemBasket))
            {
                _itemBaskets.Add(itemBasket);

                if (boxData.additional)
                {
                    Debug.Log("Additional BOX " + boxData.itemType);
                    itemBasket.LoadItems(true,boxData.amount,boxData.additionalAmountItems);
                    Debug.Log("16" );
                }
                else
                {
                    itemBasket.LoadItems(false,boxData.amount,boxData.additionalAmountItems);
                }
            }

            if (box.TryGetComponent(out ItemDrinkPackage itemDrinkPackage))
            {
                _itemDrinkPackages.Add(itemDrinkPackage);
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