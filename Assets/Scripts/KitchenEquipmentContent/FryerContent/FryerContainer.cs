using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerContainer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _items;
        [SerializeField] private ItemType _itemType;
        [FormerlySerializedAs("_positions")] [SerializeField] private Transform[] positionses;

        public event Action<int> ItemArrayValueChanged;

        public Transform[] Positions => positionses;

        public GameObject[] Items => _items;

        public ItemType ItemType => _itemType;

        public int GetInactiveValue()
        {
            List<GameObject> inactiveItems = _items.Where(p => !p.gameObject.activeSelf).ToList();
            return inactiveItems.Count;
        }

        public int GetActiveValue()
        {
            List<GameObject> inactiveItems = _items.Where(p => p.gameObject.activeSelf).ToList();
            return inactiveItems.Count;
        }

        public void ActivateItems(int value)
        {
            if (_items.Length <= 0)
            {
                Debug.LogError("_items array is not initialized.");
                return;
            }

            List<GameObject> inactiveItems = _items.Where(p => !p.gameObject.activeSelf).ToList();

            for (int i = 0; i < value; i++)
                inactiveItems[i].gameObject.SetActive(true);

            ItemArraysValueChanged();
        }

        public void DeactivateItems(int value)
        {
            if (_items == null)
            {
                Debug.LogError("_items array is not initialized.");
                return;
            }

            List<GameObject> inactiveItems = _items.Where(p => p.gameObject.activeSelf).ToList();

            for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value && i >= 0; i--)
                inactiveItems[i].gameObject.SetActive(false);

            ItemArraysValueChanged();
        }

        private void ItemArraysValueChanged()
        {
            List<GameObject> activeWell = _items.Where(p => p.gameObject.activeSelf).ToList();
            ItemArrayValueChanged?.Invoke(activeWell.Count);
        }
    }
}