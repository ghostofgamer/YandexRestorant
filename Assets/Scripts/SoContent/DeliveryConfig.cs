using Enums;
using UnityEngine;

namespace SoContent
{
    [CreateAssetMenu(fileName = "DeliveryConfig", menuName = "Configs/DeliveryConfig", order = 1)]
    public class DeliveryConfig : ScriptableObject
    {
        [SerializeField] private UnityDictionary<ItemType, GameObject> _itemPrefabs = new UnityDictionary<ItemType, GameObject>();
        [SerializeField] private int _minValueTimer;
        [SerializeField] private int _maxValueTimer;
        
        public UnityDictionary<ItemType, GameObject> ItemPrefabs => _itemPrefabs;

        public int MinValueTimer => _minValueTimer;
        public int MaxValueTimer => _maxValueTimer;
        
        public GameObject GetPrefabByItemType(ItemType itemType)
        {
            return _itemPrefabs.Get(itemType);
        }
    }
}