using Enums;
using UnityEngine;

namespace ItemContent
{
    [System.Serializable]
    public class Ingredient
    {
        public ItemType itemType;
        public Sprite sprite;
        public Sprite outlineSprite;
        public Sprite shopItemSprite;
        public int dollarsPrice;
        public int centsPrice;
        public string name;
        public string term;
    }
}