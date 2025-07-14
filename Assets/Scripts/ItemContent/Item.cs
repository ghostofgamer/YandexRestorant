using Enums;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemType _itemType;

    private Transform _defaultParentContainer;
    
    public ItemType ItemType => _itemType;

    public void SetParenContainer(Transform parent)
    {
        _defaultParentContainer = parent;
    }

    public void ReturnDefaultParent()
    {
        transform.parent = _defaultParentContainer;
        gameObject.SetActive(false);
    }
}