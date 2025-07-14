using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class DeepFryerCounterSaver : MonoBehaviour
    {
        [SerializeField] private DeepFryerItemCounter _deepFryerItemCounter;

        private void OnEnable()
        {
            _deepFryerItemCounter.ItemsValueChanged += Save;
        }

        private void OnDisable()
        {
            _deepFryerItemCounter.ItemsValueChanged -= Save;
        }

        private void Save(List<Item> items)
        {
            int[] itemTypeIndices = items.Select(item => (int)item.ItemType).ToArray();
            
            string indicesString = string.Join(",", itemTypeIndices);
            PlayerPrefs.SetString("ItemTypeDeepFryerIndices", indicesString);
            PlayerPrefs.Save();
        }
        
        public List<ItemType> LoadItemTypesFromIndices()
        {
            string indicesString = PlayerPrefs.GetString("ItemTypeDeepFryerIndices", "");
            
            int[] itemTypeIndices = indicesString.Split(',')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(int.Parse)
                .ToArray();
            
            List<ItemType> itemTypes = itemTypeIndices.Select(index => (ItemType)index).ToList();

            return itemTypes;
        }
    }
}
