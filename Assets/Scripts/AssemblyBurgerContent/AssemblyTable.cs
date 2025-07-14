using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyBurgerContent;
using AttentionHintContent;
using CameraContent;
using Enums;
using I2.Loc;
using InteractableContent;
using PlayerContent;
using SaveContent;
using SettingsContent.SoundContent;
using TutorialContent;
using UnityEngine;

public class AssemblyTable : MonoBehaviour
{
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private ItemContainer[] _itemContainers;
    [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;
    [SerializeField] private InteractableObject _interactableObject;
    [SerializeField] private Collider _collider;
    [SerializeField] private Collider[] _containerColliders;
    [SerializeField] private AssemblyBurger _assemblyBurger;
    [SerializeField] private TutorialAssemblyBurger _tutorialAssemblyBurger;

    [SerializeField] private Transform _cameraCurrentPosition;
    [SerializeField] private CameraPositionChanger _cameraPositionChanger;
    [SerializeField] private BurgersSaver _burgersSaver;

    private Dictionary<ItemType, ItemContainer> _containersByItemType;

    public event Action BurgerAssemblyBeginig;

    private void Awake()
    {
        _containersByItemType = new Dictionary<ItemType, ItemContainer>();

        foreach (var container in _itemContainers)
        {
            _containersByItemType[container.CurrentItemContainer] = container;
        }
    }

    private void OnEnable()
    {
        _interactableObject.OnAction += HandlePlayerInteraction;
    }

    private void OnDisable()
    {
        _interactableObject.OnAction -= HandlePlayerInteraction;
    }

    private void Start()
    {
        List<ItemType> itemTypes = _burgersSaver.LoadItemTypesFromIndices();

        if (itemTypes.Count > 0)
            LoadWellBurgers(itemTypes.Count, itemTypes);
    }

    public void HandlePlayerInteraction(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.CurrentDraggable != null)
        {
            ItemBasket basket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();

            if (basket != null)
            {
                ItemContainer targetContainer = GetContainerForItemType(basket.ItemType);

                if (targetContainer != null)
                {
                    if (targetContainer.IsAdditionalItemsContainer && basket.IsAdditionalItemsBasket)
                    {
                        int[] emptyPositions = targetContainer.GetEmptyPositions();
                        int[] activeItems = basket.GetActiveValueArrayItems();

                        bool hasEmptyPosition = emptyPositions != null && emptyPositions.Any(pos => pos > 0);
                        bool hasActiveItems = activeItems != null && activeItems.Any(item => item > 0);

                        if (!hasEmptyPosition)
                            AttentionHintActivator.Instance.ShowHint(
                                LocalizationManager.GetTermTranslation("No place"));
                        else if (!hasActiveItems)
                            AttentionHintActivator.Instance.ShowHint(
                                LocalizationManager.GetTermTranslation("The box is empty"));

                        if (emptyPositions.Length == activeItems.Length)
                        {
                            Debug.Log("Одинаковое коолличество видов продуктов ");

                            for (int i = 0; i < emptyPositions.Length; i++)
                            {
                                if (emptyPositions[i] > 0 && activeItems[i] > 0)
                                {
                                    int itemsToPlace = Mathf.Min(emptyPositions[i], activeItems[i]);
                                    Debug.Log("itemsToPlace " + itemsToPlace);
                                    Debug.Log("emptyPositions[i] " + emptyPositions[i]);
                                    Debug.Log("activeItems[i] " + activeItems[i]);


                                    if (_tutorial.CurrentType == TutorialType.PutBunsAssemblyTable)
                                        _tutorial.SetCurrentTutorialStage(TutorialType.PutBunsAssemblyTable);

                                    SoundPlayer.Instance.PlayPutTray();
                                    // basket.RemoveItem(itemsToPlace, i);
                                    basket.TransferProduct(itemsToPlace, i, targetContainer.AdditionalArrayPositions);
                                    targetContainer.ActivateItems(itemsToPlace, i);
                                }
                                else
                                {
                                    Debug.Log(" ЛИБо нету места или активных продуктов");
                                }
                            }
                        }
                    }
                    else
                    {
                        int emptyPosition = targetContainer.GetEmptyPosition();
                        int activeItems = basket.GetActiveValueItems();

                        if (emptyPosition <= 0)
                            AttentionHintActivator.Instance.ShowHint(
                                LocalizationManager.GetTermTranslation("No place"));
                        else if (activeItems <= 0)
                            AttentionHintActivator.Instance.ShowHint(
                                LocalizationManager.GetTermTranslation("The box is empty"));


                        if (emptyPosition > 0 && activeItems > 0)
                        {
                            SoundPlayer.Instance.PlayPutTray();
                            int itemsToPlace = Mathf.Min(emptyPosition, activeItems);
                            basket.TransferProduct(itemsToPlace, targetContainer.Positions);
                            targetContainer.ActivateItems(itemsToPlace);
                            Debug.Log($"Placed {itemsToPlace} items in container for {basket.ItemType}");

                            if (_tutorial.CurrentType == TutorialType.PutPackagesAssemblyTable)
                                _tutorial.SetCurrentTutorialStage(TutorialType.PutPackagesAssemblyTable);

                            if (_tutorial.CurrentType == TutorialType.PutWellCutletToContainer)
                            {
                                Debug.Log("!!!!!!_tutorial.CurrentType " + _tutorial.CurrentType);
                                Debug.Log("!!!!!!basket.ItemType " + basket.ItemType);

                                if (basket.ItemType == ItemType.Cutlet)
                                {
                                    Debug.Log("!!!!!!ItemType.CutletItemType.CutletItemType.Cutlet");
                                    _tutorial.SetCurrentTutorialStage(TutorialType.PutWellCutletToContainer);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log(
                                $"No space in container or no active items in basket. Container empty positions: {emptyPosition}, Basket active items: {activeItems}");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"No container found for item type: {basket.ItemType}");
                }
            }
            else
            {
                Debug.LogError("The draggable object is not an ItemBasket.");
            }
        }
        else if (playerInteraction.PlayerTray.IsActive)
        {
            ItemContainer targetContainer = GetContainerForItemType(playerInteraction.PlayerTray.CurrentType);

            if (targetContainer != null)
            {
                int emptyPosition = targetContainer.GetEmptyPosition();
                int activeItems =
                    playerInteraction.PlayerTray.GetActivePositionValue(playerInteraction.PlayerTray.CurrentType);

                if (emptyPosition > 0 && activeItems > 0)
                {
                    if (_tutorial.CurrentType == TutorialType.PutWellCutletToContainer)
                    {
                        Debug.Log("!!!!!!basket.ItemType " + playerInteraction.PlayerTray.CurrentType);

                        if (playerInteraction.PlayerTray.CurrentType == ItemType.Cutlet)
                            _tutorial.SetCurrentTutorialStage(TutorialType.PutWellCutletToContainer);
                    }

                    int itemsToPlace = Mathf.Min(emptyPosition, activeItems);
                    SoundPlayer.Instance.PlayPutTray();
                    // basket.TransferProduct(itemsToPlace, targetContainer.Positions);
                    playerInteraction.PlayerTray.PutAway(playerInteraction.PlayerTray.CurrentType, itemsToPlace);
                    targetContainer.ActivateItems(itemsToPlace);
                }
                else
                {
                    Debug.Log(
                        $"No space in container or no active items in basket. Container empty positions: {emptyPosition}, Basket active items: {activeItems}");
                }
            }
        }
        else
        {
            if ((int)_tutorial.CurrentType < (int)TutorialType.LetsMakeFirstBurger)
            {
                Debug.Log("рано тебе еще ");
                return;
            }

            if (_tutorial.CurrentType == TutorialType.LetsMakeFirstBurger)
            {
                _tutorialAssemblyBurger.StartTutorAssemblyBurger();
            }

            SoundPlayer.Instance.PlayButtonClick();
            BurgerAssemblyBeginig?.Invoke();
            SetValueCollider(false);
            _cameraPositionChanger.ChangePosition(_cameraCurrentPosition);
            Debug.Log("No draggable object in player's hands.");
        }
    }

    public void SetValueCollider(bool value)
    {
        _collider.enabled = value;
        _assemblyBurger.enabled = !value;

        foreach (var containerCollidder in _containerColliders)
        {
            containerCollidder.enabled = !value;
            Debug.Log("value " + !value);
        }
    }

    private ItemContainer GetContainerForItemType(ItemType itemType)
    {
        if (_containersByItemType.TryGetValue(itemType, out var container))
        {
            return container;
        }

        return null;
    }

    private void LoadWellBurgers(int value, List<ItemType> itemType)
    {
        StartCoroutine(StartLoad(value, itemType));
    }

    private IEnumerator StartLoad(int value, List<ItemType> itemType)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < value; i++)
        {
            _assemblyBurger.SimpleCreateStartBurgers(itemType[i]);

            /*
            Transform availablePosition = _wellPositions.FirstOrDefault(position => position.childCount == 0);
            Item sodaInstance = _burgerIngridientSpawner.SpawnItem(itemType[i]);
            sodaInstance.gameObject.SetActive(true);
            sodaInstance.transform.SetParent(availablePosition);
            sodaInstance.transform.localScale = _assemblyBurgerItemConfig.GetScale(itemType[i]);
            sodaInstance.transform.position = availablePosition.position;
            _sodaCounter.AddSoda(sodaInstance);*/
        }
    }
}