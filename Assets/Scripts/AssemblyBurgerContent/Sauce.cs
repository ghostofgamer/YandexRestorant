using Enums;
using UnityEngine;

namespace AssemblyBurgerContent
{
    public class Sauce : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;

        public ItemType ItemType => _itemType;
    }
}
