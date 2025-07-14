using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace SoContent.AssemblyBurger
{
    [CreateAssetMenu(fileName = "BurgerRecipeConfig", menuName = "Configs/BurgerRecipeConfig", order = 1)]
    public class BurgerRecipeConfig : ScriptableObject
    {
        public List<Recipes> recipes;
        
        public Recipes GetRecipeByBurgerType(ItemType burgerType)
        {
            return recipes.FirstOrDefault(recipe => recipe.BurgerType == burgerType);
        }
    }
    
    [System.Serializable]
    public class Recipes
    {
        public List<ItemType> ItemTypes;
        public ItemType BurgerType;
    }
}