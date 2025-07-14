using System;
using InteractableContent;
using UnityEngine;

namespace SaveContent
{
    public class ItemContainerSaver : MonoBehaviour
    {
        private ItemContainer _itemContainer;
        private int _firstItemsValue;
        private int _additionalItemsValue;

        private void Awake()
        {
            _itemContainer = GetComponent<ItemContainer>();
        }

        private void OnEnable()
        {
            _itemContainer.ItemsActiveCountChanged += ItemsFirstListChangedValue;
            _itemContainer.ItemsAdditionalActiveCountChanged += SaveValues;
        }

        private void OnDisable()
        {
            _itemContainer.ItemsActiveCountChanged -= ItemsFirstListChangedValue;
            _itemContainer.ItemsAdditionalActiveCountChanged -= SaveValues;
        }

        private void ItemsFirstListChangedValue(int value)
        {
            Debug.Log("value Items " + value);
            PlayerPrefs.SetInt("ItemContainer" + _itemContainer.CurrentItemContainer, value);
        }

        private void SaveValues(int firstAmountValue, int secondAmountValue)
        {
            Debug.Log("FFFFFFFFFFFFFFFFFFF " + firstAmountValue + " ???? " + secondAmountValue);


            PlayerPrefs.SetInt("ItemContainer_FirstItemsValue" + _itemContainer.CurrentItemContainer, firstAmountValue);
            PlayerPrefs.SetInt("ItemContainer_AdditionalItemsValue" + _itemContainer.CurrentItemContainer,
                secondAmountValue);
            PlayerPrefs.Save();
        }
    }
}