using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using RestaurantContent;
using UnityEngine;

namespace SaveContent
{
    public class BurgersSaver : MonoBehaviour
    {
        [SerializeField] private BurgersCounter _burgersCounter;
        
        private const string BurgerKey = "BurgerItemTypeIndices";
        
        private void OnEnable()
        {
            _burgersCounter.BurgerItemsValueChanged += SaveWellBurgers;
        }

        private void OnDisable()
        {
            _burgersCounter.BurgerItemsValueChanged -= SaveWellBurgers;
        }
        
        private void SaveWellBurgers(List<Item> items)
        {
            var wrapper = new BurgerWrapper
            {
                itemTypeIndices = items.Select(item => (int)item.ItemType).ToArray()
            };

            StorageHelper.SetObject(BurgerKey, wrapper);
            Debug.Log($"Saved Burgers: {string.Join(",", wrapper.itemTypeIndices)}");
            
            /*int[] itemTypeIndices = items.Select(item => (int)item.ItemType).ToArray();
            
            string indicesString = string.Join(",", itemTypeIndices);
            PlayerPrefs.SetString("BurgerItemTypeIndices", indicesString);
            PlayerPrefs.Save();*/
        }

        public List<ItemType> LoadItemTypesFromIndices()
        {
            if (!StorageHelper.HasKey(BurgerKey))
                return new List<ItemType>();

            var wrapper = StorageHelper.GetObject<BurgerWrapper>(BurgerKey);

            return wrapper?.itemTypeIndices
                       .Select(index => (ItemType)index)
                       .ToList()
                   ?? new List<ItemType>();
            
            
            /*string indicesString = PlayerPrefs.GetString("BurgerItemTypeIndices", "");
            
            int[] itemTypeIndices = indicesString.Split(',')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(int.Parse)
                .ToArray();
            
            List<ItemType> itemTypes = itemTypeIndices.Select(index => (ItemType)index).ToList();

            return itemTypes;*/
        }
    }
    
    [Serializable]
    public class BurgerWrapper
    {
        public int[] itemTypeIndices;
    }
}
