using Enums;
using ItemContent;
using UnityEngine;

namespace SoContent
{
    [CreateAssetMenu(fileName = "IngredientConfig", menuName = "Configs/IngredientConfig", order = 1)]
    public class IngredientsConfig : ScriptableObject
    {
        [SerializeField] private Ingredient[] _ingredients;

        public Sprite GetSprite(ItemType itemType)
        {
            foreach (var ingredient in _ingredients)
            {
                if (ingredient.itemType == itemType)
                    return ingredient.sprite;
            }

            Debug.LogWarning($"Sprite for ItemType {itemType} not found.");
            return null;
        }
        
        public Sprite GetOutlineSprite(ItemType itemType)
        {
            foreach (var ingredient in _ingredients)
            {
                if (ingredient.itemType == itemType)
                    return ingredient.outlineSprite;
            }

            Debug.LogWarning($"Sprite for ItemType {itemType} not found.");
            return null;
        }

        public Ingredient GetIngredient(ItemType itemType)
        {
            foreach (var ingredient in _ingredients)
            {
                if (ingredient.itemType == itemType)
                    return ingredient;
            }

            Debug.LogWarning($"Sprite for ItemType {itemType} not found.");
            return null;
        }
    }
}