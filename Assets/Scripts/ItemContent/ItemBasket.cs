using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using ItemContent;
using ShelfContent;
using UnityEngine;

public class ItemBasket : MonoBehaviour
{
    [SerializeField] private Draggable _draggable;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private Item[] _items;
    [SerializeField] private ActivityItem[] _itemsActivity;
    [SerializeField] private Item[] _additionalItems;
    [SerializeField] private ActivityItem[] _additionalItemsActivity;
    [SerializeField] private bool _isAdditionalItemsBasket;
    [SerializeField] private ItemType[] _currentItemsType;
    [SerializeField] private Transform[] _positions;

    [SerializeField] private Item[][] _itemsAdditionalArray;
    [SerializeField] private ActivityItem[][] _itemsActivityAdditionalArray;

    private bool _firstLoad = true;

    public Shelf Shelf { get; private set; }
    public Transform[] Positions => _positions;
    public ItemType ItemType => _itemType;
    public bool IsAdditionalItemsBasket => _isAdditionalItemsBasket;
    public ItemType[] CurrentItemsTypes => _currentItemsType;

    public event Action TransferProductsEnded;

    private void OnEnable()
    {
        _draggable.DraggablePicked += ActivateItems;
        _draggable.DraggableThrowed += DeactivateItems;
        _draggable.PutOnShelfCompleting += DeactivateItems;
    }

    private void OnDisable()
    {
        _draggable.DraggablePicked -= ActivateItems;
        _draggable.DraggableThrowed -= DeactivateItems;
        _draggable.PutOnShelfCompleting -= DeactivateItems;
    }

    private void Start()
    {
        if (!_firstLoad)
            return;

        _itemsAdditionalArray = new Item[][] { _items, _additionalItems };
        _itemsActivityAdditionalArray = new ActivityItem[][] { _itemsActivity, _additionalItemsActivity };
        DeactivateItems();
    }

    public int GetActiveValueItems()
    {
        int activeCount = 0;

        foreach (var item in _itemsActivity)
        {
            if (item != null && item.IsActive)
                activeCount++;
        }

        return activeCount;
    }

    public int[] GetActiveValueArrayItems()
    {
        int[] activeCounts = new int[_itemsActivityAdditionalArray.Length];

        for (int i = 0; i < _itemsActivityAdditionalArray.Length; i++)
        {
            int rowActiveCount = 0;
            var itemsRow = _itemsActivityAdditionalArray[i];

            foreach (var item in itemsRow)
            {
                if (item != null && item.IsActive)
                    rowActiveCount++;
            }

            activeCounts[i] = rowActiveCount;
        }

        return activeCounts;
    }

    public void RemoveItem(int value)
    {
        if (_items == null)
        {
            Debug.LogError("_items array is not initialized.");
            return;
        }

        List<ActivityItem> inactiveItems = _itemsActivity.Where(p => p.IsActive).ToList();

        if (value > inactiveItems.Count)
            value = inactiveItems.Count;

        for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value; i--)
            inactiveItems[i].SetValue(false);
    }

    public void TransferProduct(int value, Transform[] positions)
    {
        if (_items == null)
        {
            Debug.LogError("_items array is not initialized.");
            return;
        }

        List<ActivityItem> inactiveItems = _itemsActivity.Where(p => p.IsActive).ToList();

        if (value > inactiveItems.Count)
            value = inactiveItems.Count;

        int completedAnimations = 0;
        int totalAnimations = value;

        for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value; i--)
        {
            int index = i;

            inactiveItems[index].transform.DOMove(positions[index].transform.position, 0.15f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    inactiveItems[index].transform.localPosition = Vector3.zero;
                    inactiveItems[index].SetValue(false);

                    completedAnimations++;

                    if (completedAnimations == totalAnimations)
                        TransferProductsEnded?.Invoke();
                });
        }
    }

    public void TransferProduct(int value, int index, Transform[][] positions)
    {
        if (_itemsActivityAdditionalArray == null || _itemsActivityAdditionalArray[index] == null)
        {
            Debug.LogError("_itemsActivityAdditionalArray array is not initialized.");
            return;
        }

        List<ActivityItem> inactiveItems = _itemsActivityAdditionalArray[index].Where(p => p.IsActive).ToList();

        if (value > inactiveItems.Count)
            value = inactiveItems.Count;

        int completedAnimations = 0;
        int totalAnimations = value;

        for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value; i--)
        {
            Debug.Log("ААА " + i);

            int itemIndex = i;

            inactiveItems[itemIndex].transform.DOMove(positions[index][itemIndex].transform.position, 0.15f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    inactiveItems[itemIndex].transform.localPosition = Vector3.zero;
                    inactiveItems[itemIndex].SetValue(false);

                    completedAnimations++;

                    if (completedAnimations == totalAnimations)
                    {
                        TransferProductsEnded?.Invoke();
                    }
                });
        }
    }

    public void RemoveItem(int value, int index)
    {
        if (_itemsActivityAdditionalArray[index] == null)
        {
            Debug.LogError("_itemsActivityAdditionalArray array is not initialized.");
            return;
        }

        List<ActivityItem> inactiveItems = _itemsActivityAdditionalArray[index].Where(p => p.IsActive).ToList();

        if (value > inactiveItems.Count)
            value = inactiveItems.Count;

        for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value; i--)
        {
            inactiveItems[i].SetValue(false);
        }
    }

    public void SetActiveValue(bool value)
    {
        foreach (var item in _itemsActivity)
            if (item != null & item.IsActive)
                item.gameObject.SetActive(value);

        foreach (var item in _additionalItemsActivity)
            if (item != null & item.IsActive)
                item.gameObject.SetActive(value);
    }

    private void ActivateItems()
    {
        SetActiveValue(true);

        if (Shelf != null)
        {
            Shelf.Remove(this);
            Shelf = null;
        }
    }

    private void DeactivateItems()
    {
        SetActiveValue(false);
    }

    public void LoadItems(bool additional, int amountItems, List<int> additionalAmountItems)
    {
        if (!additional)
        {
            foreach (var item in _itemsActivity)
                item.SetValue(false);

            for (int i = 0; i < amountItems; i++)
                _itemsActivity[i].SetValue(true);
        }
        else
        {
            _firstLoad = false;
            _itemsAdditionalArray = new Item[][] { _items, _additionalItems };
            _itemsActivityAdditionalArray = new ActivityItem[][] { _itemsActivity, _additionalItemsActivity };
            DeactivateItems();

            foreach (var row in _itemsActivityAdditionalArray)
            {
                foreach (var item in row)
                    item.SetValue(false);
            }

            for (int i = 0; i < additionalAmountItems.Count; i++)
            {
                if (i < _itemsActivityAdditionalArray.Length)
                {
                    int count = additionalAmountItems[i];

                    for (int j = 0; j < count && j < _itemsActivityAdditionalArray[i].Length; j++)
                        _itemsActivityAdditionalArray[i][j].SetValue(true);
                }
            }

            DeactivateItems();
        }
    }

    public void SetShelf(Shelf shelf)
    {
        Shelf = shelf;
    }
}