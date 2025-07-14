using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerTool : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private GameObject[] _rawItemObjects;
        [SerializeField] private GameObject[] _wellItemObjects;
        [SerializeField] private Transform[] _positions;
        [SerializeField] private int _maxCount;
        [SerializeField] private FryerToolMover _fryerToolMover;
        [SerializeField] private GameObject _effectFry;

        public event Action<int, int> ItemsValueChanged;

        public GameObject[] WellItems => _wellItemObjects;

        public Transform[] Positions => _positions;

        public ItemType ItemType => _itemType;

        public bool IsFull { get; private set; }

        public bool IsRaw { get; private set; } = true;

        public void Init(int rawCount, int wellCount)
        {
            AllRawItemsDeactivate();
            AllWellItemsDeactivate();

            if (rawCount > 0)
                ActivateRawItems(rawCount);

            if (wellCount > 0)
                ActivateWellItems(wellCount);
        }

        public int GetCountActiveItems()
        {
            List<GameObject> activeItems = _rawItemObjects.Where(p => p.gameObject.activeSelf).ToList();
            return activeItems.Count;
        }

        public int GetCountActiveWellItems()
        {
            List<GameObject> activeItems = _wellItemObjects.Where(p => p.gameObject.activeSelf).ToList();
            return activeItems.Count;
        }

        public int GetCountInactiveItems()
        {
            List<GameObject> inactiveItems = _rawItemObjects.Where(p => !p.gameObject.activeSelf).ToList();
            return inactiveItems.Count;
        }

        public void AllRawItemsDeactivate()
        {
            foreach (var rawItemObject in _rawItemObjects)
                rawItemObject.SetActive(false);

            ItemArraysValueChanged();
        }

        public void AllWellItemsDeactivate()
        {
            foreach (var wellItemObject in _wellItemObjects)
                wellItemObject.SetActive(false);

            IsRaw = true;

            ItemArraysValueChanged();
        }

        public void ActivateRawItems(int value)
        {
            if (_rawItemObjects.Length <= 0)
            {
                Debug.LogError("_items array is not initialized.");
                return;
            }

            List<GameObject> inactiveItems = _rawItemObjects.Where(p => !p.gameObject.activeSelf).ToList();

            for (int i = 0; i < value; i++)
                inactiveItems[i].gameObject.SetActive(true);

            ItemArraysValueChanged();

            /*List<GameObject> activeItems = itemObjects.Where(p => p.gameObject.activeSelf).ToList();
            if()*/

            /*List<Item> activeItems = _items.Where(p => p.gameObject.activeSelf).ToList();
            ItemsActiveCountChanged?.Invoke(activeItems.Count);*/
        }

        public void ActivateWellItems()
        {
            int value = _rawItemObjects.Count(p => p.gameObject.activeSelf);
            AllRawItemsDeactivate();
            AllWellItemsDeactivate();

            for (int i = 0; i < value; i++)
                _wellItemObjects[i].SetActive(true);

            IsRaw = false;

            ItemArraysValueChanged();
        }

        public void ActivateWellItems(int value)
        {
            for (int i = 0; i < value; i++)
                _wellItemObjects[i].SetActive(true);

            IsRaw = false;

            ItemArraysValueChanged();
        }

        public void DeactivateWellItems(int value)
        {
            List<GameObject> activeItems = _wellItemObjects.Where(p => p.gameObject.activeSelf).ToList();

            for (int i = activeItems.Count - 1; i >= 0 && value > 0; i--, value--)
                activeItems[i].gameObject.SetActive(false);

            List<GameObject> active = _wellItemObjects.Where(p => p.gameObject.activeSelf).ToList();

            if (active.Count <= 0)
                IsRaw = true;

            ItemArraysValueChanged();
        }

        public void MoveFrying()
        {
            _fryerToolMover.MoveFrying();
            _effectFry.SetActive(true);
        }

        private void ItemArraysValueChanged()
        {
            List<GameObject> activeRaw = _rawItemObjects.Where(p => p.gameObject.activeSelf).ToList();
            List<GameObject> activeWell = _wellItemObjects.Where(p => p.gameObject.activeSelf).ToList();

            ItemsValueChanged?.Invoke(activeRaw.Count, activeWell.Count);
        }

        public void ResetPosition()
        {
            foreach (var item in _wellItemObjects)
                item.transform.localPosition = Vector3.zero;
            
            foreach (var item in _rawItemObjects)
                item.transform.localPosition = Vector3.zero;
        }
    }
}