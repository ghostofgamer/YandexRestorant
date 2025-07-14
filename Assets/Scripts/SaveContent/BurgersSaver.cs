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
            int[] itemTypeIndices = items.Select(item => (int)item.ItemType).ToArray();
            
            string indicesString = string.Join(",", itemTypeIndices);
            PlayerPrefs.SetString("BurgerItemTypeIndices", indicesString);
            PlayerPrefs.Save();
        }

        public List<ItemType> LoadItemTypesFromIndices()
        {
            string indicesString = PlayerPrefs.GetString("BurgerItemTypeIndices", "");
            
            int[] itemTypeIndices = indicesString.Split(',')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(int.Parse)
                .ToArray();
            
            List<ItemType> itemTypes = itemTypeIndices.Select(index => (ItemType)index).ToList();

            return itemTypes;
        }
    }
}
