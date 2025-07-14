using UnityEngine;
using WalletContent;

namespace SoContent
{
    [CreateAssetMenu(fileName = "NewShelfConfigs", menuName = "Configs/ShelfConfigs")]
    public class ShelfConfigs : ScriptableObject
    {
        public ShelfConfig[] shelves;
    }

    [System.Serializable]
    public class ShelfConfig
    {
        public int index;
        public bool storage1ToUnlock;
        public DollarValue price;
        public string name;
    }
}