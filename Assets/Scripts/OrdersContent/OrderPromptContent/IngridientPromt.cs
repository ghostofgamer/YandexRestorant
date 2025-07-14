using System.Linq;
using AssemblyBurgerContent;
using Enums;
using IngredientsContent;
using OrdersContent;
using SoContent;
using SoContent.AssemblyBurger;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngridientPromt : MonoBehaviour
{
    [SerializeField] private AssemblyBurger _assemblyBurger;
    [SerializeField] private Image[] _ingredients;
    [SerializeField] private BurgerRecipeConfig _burgerRecipeConfig;
    [SerializeField] private IngredientsConfig _ingredientsConfig;
    [SerializeField] private ElementSelector _elementSelector;
    [SerializeField] private IngredientsViewer[] _ingredientsViewers;
    [SerializeField]private Sprite _burgerPackageSprite;

    private Recipes _recipes;
    private Order _order;

    private void OnEnable()
    {
        _assemblyBurger.StackChanged += CheckIngredientsProgress;
    }

    private void OnDisable()
    {
        _assemblyBurger.StackChanged -= CheckIngredientsProgress;
    }

    public void SetIngredients(Order order)
    {
        _order = order;

        foreach (var ingredient in _ingredientsViewers)
        {
            ingredient.DeactivationOutline();
            ingredient.gameObject.SetActive(false);
        }

        if (order.BurgerItemOrder != ItemType.Empty && !order.IsBurgerCompleted)
        {
            _recipes = _burgerRecipeConfig.GetRecipeByBurgerType(order.BurgerItemOrder);

            for (int i = _recipes.ItemTypes.Count - 1, j = 0; i >= 0; i--, j++)
            {
                _ingredientsViewers[j].SetDefault(_ingredientsConfig.GetSprite(_recipes.ItemTypes[i]));
                _ingredientsViewers[j].SetItemType(_recipes.ItemTypes[i]);
                _ingredientsViewers[j].gameObject.SetActive(true);
            }

            CheckIngredientsProgress();
        }
        else if (order.IsBurgerCompleted && order.DrinkItemOrder != ItemType.Empty && !order.IsDrinkCompleted)
        {
            _elementSelector.ReturnDefaultSpacing();

            _ingredientsViewers[0].SetDefault(_ingredientsConfig.GetSprite(_order.DrinkItemOrder));
            _ingredientsViewers[0].gameObject.SetActive(true);
            _ingredientsViewers[0].SetDefaultColor(Color.white);
            return;
        }
        else if (order.IsBurgerCompleted && order.IsDrinkCompleted && !order.IsExtraCompleted &&
                 order.ExtraItemOrder != ItemType.Empty)
        {
            _elementSelector.ReturnDefaultSpacing();
        }
    }

    public void CheckIngredientsProgress()
    {
        if (_order != null)
        {
            if (_order.IsBurgerCompleted)
            {
                foreach (var ingredient in _ingredientsViewers)
                    ingredient.gameObject.SetActive(false);

                if (!_order.IsDrinkCompleted)
                {
                    _elementSelector.ReturnDefaultSpacing();
                    
                    _ingredientsViewers[0].SetDefault(_ingredientsConfig.GetSprite(_order.DrinkItemOrder));
                    _ingredientsViewers[0].gameObject.SetActive(true);
                    _ingredientsViewers[0].SetDefaultColor(Color.white);
                    return;
                }
            }
            else
            {
                foreach (var ingredient in _ingredientsViewers)
                    ingredient.SetDefaultColor(Color.white);
            }

            foreach (var ingredient in _ingredientsViewers)
                ingredient.SetOutlineBackground(false, null);

            _elementSelector.ReturnDefaultSpacing();
        }
        
        if (_assemblyBurger.IngredientStack.Count == 0)
        {
            _elementSelector.SetSpacing(_recipes.ItemTypes.Count);
            _ingredientsViewers[0]
                .SetOutlineBackground(true, _ingredientsConfig.GetOutlineSprite(_ingredientsViewers[0].ItemType));

            _elementSelector.UpdateSpacing(0, _recipes.ItemTypes.Count, 1);
            return;
        }

        if (_assemblyBurger.IngredientStack.Count > _recipes.ItemTypes.Count)
        {
            foreach (var ingredient in _ingredientsViewers)
                ingredient.SetDefaultColor(Color.gray);

            return;
        }

        var stackItems = _assemblyBurger.IngredientStack.Select(x => x.item.ItemType).ToList();
        stackItems.Reverse();
        int lastCorrectIndex = -1;

        for (int i = 0; i < _ingredientsViewers.Length; i++)
        {
            int recipeIndex = _recipes.ItemTypes.Count - 1 - i;

            if (i >= stackItems.Count)
            {
                Debug.LogError($"{i}). Индекс выходит за пределы стека");
                continue;
            }

            if (stackItems[i] == _recipes.ItemTypes[recipeIndex])
            {
                _ingredientsViewers[i].SetDefaultColor(Color.gray);
                _elementSelector.UpdateSpacing(i, _recipes.ItemTypes.Count);
                lastCorrectIndex = i;

                if (i == (stackItems.Count - 1) && (i + 1) < _ingredientsViewers.Count())
                {
                    _ingredientsViewers[i + 1].SetOutlineBackground(true,
                        _ingredientsConfig.GetOutlineSprite(_ingredientsViewers[i + 1].ItemType));
                }
            }
            else
            {
                foreach (var ingredient in _ingredientsViewers)
                    ingredient.SetDefaultColor(Color.gray);

                _elementSelector.ReturnDefaultSpacing();
                return;
            }
        }
    }
}