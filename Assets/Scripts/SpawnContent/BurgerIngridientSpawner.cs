using System.Collections.Generic;
using Enums;
using UnityEngine;

public class BurgerIngridientSpawner : MonoBehaviour
{
    [SerializeField] private Item[] _prefabs;
    [SerializeField] private Transform _container;
    
    private Dictionary<ItemType, ObjectPool<Item>> _objectPools;

    private void Start()
    {
        _objectPools = new Dictionary<ItemType, ObjectPool<Item>>();
        
        foreach (var prefab in _prefabs)
        {
            ItemType itemType = prefab.GetComponent<Item>().ItemType;
            ObjectPool<Item> pool = new ObjectPool<Item>(prefab.GetComponent<Item>(), 15, _container);
            pool.EnableAutoExpand();
            _objectPools[itemType] = pool;
        }
    }
    
    public Item SpawnItem(ItemType itemType)
    {
        if (_objectPools.TryGetValue(itemType, out ObjectPool<Item> pool))
        {
            return pool.GetFirstObject();
        }
        else
        {
            Debug.LogError($"Pool for item type {itemType} not found.");
            return null;
        }
    }
}