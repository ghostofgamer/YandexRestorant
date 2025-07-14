using System;
using System.Collections.Generic;
using Enums;
using SoContent;
using UnityEngine;

namespace RestaurantContent.MenuContent
{
    public class MenuCounter : MonoBehaviour
    {
        [SerializeField] private List<ItemType> _menuList;
        [SerializeField] private ItemsConfig _itemsConfig;

        private Dictionary<ItemType, List<ItemType>> _categoryDictionary;

        public List<ItemType> MenuList => _menuList;
        private List<ItemType> _cachedBurgers = new List<ItemType>();
        private List<ItemType> _cachedDrinks = new List<ItemType>();
        private List<ItemType> _cachedExtras = new List<ItemType>();

        public event Action<List<ItemType>> ChangeMenuList;
        
        private void Awake()
        {
            Debug.Log("Awake");
            _itemsConfig.Initialize();
            _categoryDictionary = new Dictionary<ItemType, List<ItemType>>();
            CategorizeMenuItems();
        }

        public void AddItem(ItemType itemType)
        {
            if (!_menuList.Contains(itemType))
            {
                _menuList.Add(itemType);
                UpdateCachedListsForItem(itemType, true);
                ChangeMenuList?.Invoke(_menuList);
            }
            else
            {
                Debug.Log($"{itemType} is already in the menu.");
            }
        }

        public void RemoveItem(ItemType itemType)
        {
            if (_menuList.Contains(itemType))
            {
                _menuList.Remove(itemType);
                UpdateCachedListsForItem(itemType, false);
                ChangeMenuList?.Invoke(_menuList);
            }
            else
            {
                Debug.Log($"{itemType} is not in the menu.");
            }
        }
        
        private void UpdateCachedList(List<ItemType> cachedList, ItemType itemType, bool isAdding)
        {
            if (isAdding)
            {
                cachedList.Add(itemType);
            }
            else
            {
                cachedList.Remove(itemType);
            }
        }
        
        private void UpdateCachedListsForItem(ItemType itemType, bool isAdding)
        {
            var itemConfig = _itemsConfig.GetItemConfig(itemType);
            if (itemConfig != null)
            {
                switch (itemConfig.Category)
                {
                    case ItemType.BurgerItemOrder:
                        UpdateCachedList(_cachedBurgers, itemType, isAdding);
                        break;
                    case ItemType.DrinkItemOrder:
                        UpdateCachedList(_cachedDrinks, itemType, isAdding);
                        break;
                    case ItemType.ExtraItemOrder:
                        UpdateCachedList(_cachedExtras, itemType, isAdding);
                        break;
                }
            }
        }

        public void CategorizeMenuItems()
        {
            _categoryDictionary.Clear();
            _cachedBurgers.Clear();
            _cachedDrinks.Clear();
            _cachedExtras.Clear();

            foreach (var itemType in _menuList)
            {
                var itemConfig = _itemsConfig.GetItemConfig(itemType);

                if (itemConfig != null)
                {
                    if (!_categoryDictionary.ContainsKey(itemConfig.Category))
                    {
                        _categoryDictionary[itemConfig.Category] = new List<ItemType>();
                    }

                    _categoryDictionary[itemConfig.Category].Add(itemType);

                    switch (itemConfig.Category)
                    {
                        case ItemType.BurgerItemOrder:
                            _cachedBurgers.Add(itemType);
                            break;
                        case ItemType.DrinkItemOrder:
                            _cachedDrinks.Add(itemType);
                            break;
                        case ItemType.ExtraItemOrder:
                            _cachedExtras.Add(itemType);
                            break;
                    }
                }
            }

            foreach (var category in _categoryDictionary)
            {
                Debug.Log($"Category: {category.Key}");
                foreach (var item in category.Value)
                {
                    Debug.Log($" - {item}");
                }
            }
        }
        
        public List<ItemType> GetBurgers()
        {
            return _cachedBurgers;
        }

        public List<ItemType> GetDrinks()
        {
            return _cachedDrinks;
        }

        public List<ItemType> GetExtras()
        {
            return _cachedExtras;
        }
    }
}