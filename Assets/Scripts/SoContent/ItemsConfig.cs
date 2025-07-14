using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace SoContent
{
    [CreateAssetMenu(fileName = "NewItemsConfig", menuName = "Configs/ItemsConfig")]
    public class ItemsConfig : ScriptableObject
    {
        public List<ItemConfig> items;

        private Dictionary<ItemType, ItemConfig> _itemDictionary;

        public void Initialize()
        {
            _itemDictionary = new Dictionary<ItemType, ItemConfig>();
            
            foreach (var item in items)
            {
                _itemDictionary[item.ItemType] = item;
            }
        }

        public ItemConfig GetItemConfig(ItemType itemType)
        {
            _itemDictionary.TryGetValue(itemType, out var itemConfig);
            return itemConfig;
        }
    }
}