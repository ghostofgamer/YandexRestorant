using Enums;
using InteractableContent;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class TriggerContainer : MonoBehaviour
    {
        [SerializeField] private ItemContainer _itemContainer;
        [SerializeField] private ItemType _itemType;

        public ItemType ItemType => _itemType;

        public ItemContainer ItemContainer => _itemContainer;
    }
}