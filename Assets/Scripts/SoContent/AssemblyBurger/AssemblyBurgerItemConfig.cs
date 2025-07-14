using Enums;
using UnityEngine;

namespace SoContent.AssemblyBurger
{
    [System.Serializable]
    public class ItemScaleData
    {
        public ItemType itemType;
        public Vector3 scale;
    }
    
    [CreateAssetMenu(fileName = "ItemScaleConfig", menuName = "Configs/ItemScaleConfig", order = 1)]
    public class AssemblyBurgerItemConfig : ScriptableObject
    {
        public ItemScaleData[] itemScales;
        
        public Vector3 GetScale(ItemType itemType)
        {
            foreach (var itemScale in itemScales)
            {
                if (itemScale.itemType == itemType)
                {
                    return itemScale.scale;
                }
            }
          
            return Vector3.one;
        }
    }
}
