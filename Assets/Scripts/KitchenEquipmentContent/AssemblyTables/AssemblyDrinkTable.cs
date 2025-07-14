using System;
using CameraContent;
using Enums;
using InteractableContent;
using ItemContent;
using PlayerContent;
using UnityEngine;

namespace KitchenEquipmentContent
{
    public abstract class AssemblyDrinkTable : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private ItemContainer _itemContainer;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private Transform _cameraCurrentPosition;
        [SerializeField] private Collider _collider;
        [SerializeField] private ItemType[] _currentWellTypes;
        public event Action DrinkAssemblyBeginig;

        public ItemContainer ItemContainer => _itemContainer;

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.CurrentDraggable != null)
            {
                ItemBasket basket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();
                ItemDrinkPackage drinkPackage = playerInteraction.CurrentDraggable.GetComponent<ItemDrinkPackage>();

                if (basket != null)
                {
                    if (basket.ItemType == _itemContainer.CurrentItemContainer)
                    {
                        Debug.Log("Тип продукта в коробке можно положить ");

                        int emptyPosition = _itemContainer.GetEmptyPosition();
                        int activeItems = basket.GetActiveValueItems();

                        if (emptyPosition > 0 && activeItems > 0)
                        {
                            int itemsToPlace = Mathf.Min(emptyPosition, activeItems);
                            basket.TransferProduct(itemsToPlace, _itemContainer.Positions);
                            _itemContainer.ActivateItems(itemsToPlace);
                            Debug.Log($"Placed {itemsToPlace} items in container for {basket.ItemType}");
                        }
                        else
                        {
                            Debug.Log(
                                $"No space in container or no active items in basket. Container empty positions: {emptyPosition}, Basket active items: {activeItems}");
                        }
                    }
                    else
                    {
                        Debug.Log("Тип продукта в коробке нельзя разместить тут");
                    }
                }
                else if (drinkPackage != null)
                {
                    foreach (var wellType in _currentWellTypes)
                    {
                        if (drinkPackage.ItemType == wellType)
                            FillDrinkMachine(drinkPackage);
                    }
                }
                else
                {
                    Debug.Log("The draggable object is not an ItemBasket.");
                }
            }
            else if (playerInteraction.PlayerTray.IsActive)
            {
                return;
            }
            else
            {
                DrinkAssemblyBeginig?.Invoke();
                SetValueCollider(false);
                _cameraPositionChanger.ChangePosition(_cameraCurrentPosition);

                Debug.Log("No draggable object in player's hands.");
            }
        }

        public void SetValueCollider(bool value)
        {
            _collider.enabled = value;
        }

        public abstract void FillDrinkMachine(ItemDrinkPackage itemDrinkPackage);
    }
}