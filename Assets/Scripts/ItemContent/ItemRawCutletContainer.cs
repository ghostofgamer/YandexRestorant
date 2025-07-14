using AttentionHintContent;
using DG.Tweening;
using Enums;
using I2.Loc;
using InteractableContent;
using PlayerContent;
using SoContent.AssemblyBurger;
using TutorialContent;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace ItemContent
{
    public class ItemRawCutletContainer : ItemContainer
    {
        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;
        [SerializeField] private AssemblyBurgerItemConfig _assemblyBurgerItemConfig;
        [SerializeField] private Tutorial _tutorial;

        public override void ActionContainer(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.CurrentDraggable != null)
            {
                Debug.Log($"ELSE ELSE ELSE CurrentDraggable!=null");
                
                ItemBasket basket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();

                if (basket != null && basket.ItemType == ItemType.RawCutlet)
                {
                    int emptyPosition = GetEmptyPosition();
                    int activeItems = basket.GetActiveValueItems();
                    
                    if (emptyPosition <= 0)
                        AttentionHintActivator.Instance.ShowHint(
                            LocalizationManager.GetTermTranslation("No place"));
                    else if (activeItems <= 0)
                        AttentionHintActivator.Instance.ShowHint(LocalizationManager.GetTermTranslation("The box is empty"));
                    
                    if (emptyPosition > 0 && activeItems > 0)
                    {
                        if (_tutorial.CurrentType == TutorialType.PutRawCutletInContainer)
                            _tutorial.SetCurrentTutorialStage(TutorialType.PutRawCutletInContainer);


                        int itemsToPlace = Mathf.Min(emptyPosition, activeItems);
                        basket.TransferProduct(itemsToPlace, Positions);
                        ActivateItems(itemsToPlace);
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
                    Debug.Log($"basket  NULL");
                }
            }
            else if (playerInteraction.PlayerTray)
            {
                if (playerInteraction.PlayerTray.CurrentType == ItemType.RawCutlet ||
                    playerInteraction.PlayerTray.CurrentType == ItemType.Empty)
                {
                    if (!playerInteraction.PlayerTray.IsActive)
                    {
                        int activeItems = GetActiveItemsValue();
                        
                        if (activeItems <= 0)
                            return;
                        
                        Debug.Log("GetActiveItemsValue " + activeItems);
                        int emptyPos = playerInteraction.PlayerTray.GetEmptyPositionValue(CurrentItemContainer);
                        int itemsToPlace = Mathf.Min(activeItems, emptyPos);

                        Debug.Log("GetEmptyPositionValue " + emptyPos);
                        Debug.Log("itemsToPlace " + itemsToPlace);
                        
                        if (itemsToPlace > 0)
                        {
                            DeactivateItems(itemsToPlace);


                            int completedAnimations = 0;
                            Vector3 scale = _assemblyBurgerItemConfig.GetScale(ItemType.RawCutlet);

                            for (int i = 0; i < itemsToPlace; i++)
                            {
                                Item item = _burgerIngridientSpawner.SpawnItem(ItemType.RawCutlet);
                                item.gameObject.SetActive(true);
                                item.transform.position = Positions[i].transform.position;

                                item.transform.localScale = scale;

                                Sequence sequence = DOTween.Sequence();
                                sequence.Append(item.transform
                                    .DOMove(playerInteraction.PlayerTray.transform.position, 0.15f)
                                    .SetEase(Ease.InOutQuad));
                                sequence.Join(item.transform
                                    .DORotate(playerInteraction.PlayerTray.transform.eulerAngles, 0.15f)
                                    .SetEase(Ease.InOutQuad));
                                sequence.OnComplete(() =>
                                {
                                    completedAnimations++;
                                    item.transform.position = Vector3.zero;
                                    item.gameObject.SetActive(false);

                                    if (completedAnimations == itemsToPlace)
                                    {
                                        playerInteraction.PlayerTray.Put(CurrentItemContainer, itemsToPlace);
                                    }
                                });
                            }

                            // DeactivateItems(itemsToPlace);
                            // playerInteraction.PlayerTray.Put(CurrentItemContainer, itemsToPlace);
                            Debug.Log($"itemsToPlace " + itemsToPlace);
                        }
                    }
                    else
                    {
                        Debug.Log($"ELSE ELSE ELSE ");
                        
                        int emptyPosition = GetEmptyPosition();
                        int activeItems = playerInteraction.PlayerTray.GetActivePositionValue(CurrentItemContainer);
                        int itemsToPlace = Mathf.Min(emptyPosition, activeItems);


                        if (itemsToPlace > 0)
                        {
                            playerInteraction.PlayerTray.PutAway(CurrentItemContainer, itemsToPlace);
                            // playerInteraction.PlayerTray.PutAway(CurrentItemContainer, itemsToPlace);

                            int completedAnimations = 0;
                            Vector3 scale = _assemblyBurgerItemConfig.GetScale(ItemType.RawCutlet);

                            for (int i = 0; i < itemsToPlace; i++)
                            {
                                Item item = _burgerIngridientSpawner.SpawnItem(ItemType.RawCutlet);
                                item.gameObject.SetActive(true);

                                item.transform.position = playerInteraction.PlayerTray.Positions[i].position;
                                item.transform.localScale = scale;

                                Sequence sequence = DOTween.Sequence();

                                sequence.Append(item.transform
                                    .DOMove(Positions[i].position, 0.15f)
                                    .SetEase(Ease.InOutQuad));
                                sequence.Join(item.transform
                                    .DORotate(transform.eulerAngles, 0.15f)
                                    .SetEase(Ease.InOutQuad));
                                sequence.OnComplete(() =>
                                {
                                    completedAnimations++;
                                    item.transform.position = Vector3.zero;
                                    item.gameObject.SetActive(false);

                                    if (completedAnimations == itemsToPlace)
                                    {
                                        ActivateItems(itemsToPlace);
                                    }
                                });
                            }

                            // ActivateItems(itemsToPlace);
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"Current Draggable  And Tray NULL");
            }
        }
    }
}