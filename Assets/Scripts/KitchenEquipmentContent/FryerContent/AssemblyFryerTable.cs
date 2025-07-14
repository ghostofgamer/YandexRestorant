using System;
using System.Collections;
using System.Collections.Generic;
using AttentionHintContent;
using CameraContent;
using Enums;
using I2.Loc;
using InteractableContent;
using PlayerContent;
using SettingsContent.SoundContent;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class AssemblyFryerTable : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private FryerContainer[] _fryerContainers;
        [SerializeField] private FryerFrying _fryerFrying;
        [SerializeField] private FryerTool[] _fryerTools;
        [SerializeField] private ItemContainer[] _itemContainersPackage;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private Transform _cameraCurrentPosition;
        [SerializeField] private Collider _collider;
        [SerializeField] private Collider[] _containerColliders;
        [SerializeField] private AssemblyFromDeepFry _assemblyFromDeepFry;
        [SerializeField] private DeepFryerCounterSaver _deepFryerCounterSaver;
        [SerializeField] private TransferItems _transferItems;

        public event Action FriersAssemblyBeginig;

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
            _fryerFrying.FryCompleted += FillTable;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
            _fryerFrying.FryCompleted -= FillTable;
        }

        private void Start()
        {
            List<ItemType> itemTypes = _deepFryerCounterSaver.LoadItemTypesFromIndices();

            if (itemTypes.Count > 0)
                LoadWellItems(itemTypes.Count, itemTypes);
            else
                FillTable();
        }

        private void LoadWellItems(int value, List<ItemType> itemType)
        {
            StartCoroutine(StartLoad(value, itemType));
        }

        private IEnumerator StartLoad(int value, List<ItemType> itemType)
        {
            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < value; i++)
                _assemblyFromDeepFry.SimpleCreatItem(itemType[i]);

            FillTable();
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            Debug.Log("сборочный стол фритюрницы");

            if (playerInteraction.CurrentDraggable != null)
            {
                Debug.Log("В руках есть");

                ItemBasket itemBasket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();

                if (itemBasket != null)
                {
                    foreach (var itemContainer in _itemContainersPackage)
                    {
                        if (itemBasket.ItemType == itemContainer.CurrentItemContainer)
                        {
                            Debug.Log("Совпадение контенера и коробки по типу " + itemBasket.ItemType);

                            int emptyPosContainer = itemContainer.GetEmptyPosition();
                            int activeItemBasket = itemBasket.GetActiveValueItems();
                            
                            if (emptyPosContainer <= 0)
                                AttentionHintActivator.Instance.ShowHint(
                                    LocalizationManager.GetTermTranslation("No place"));
                            else if (activeItemBasket <= 0)
                                AttentionHintActivator.Instance.ShowHint(
                                    LocalizationManager.GetTermTranslation("The box is empty"));

                            if (emptyPosContainer > 0 && activeItemBasket > 0)
                            {
                                int itemsToPlace = Mathf.Min(emptyPosContainer, activeItemBasket);
                                itemBasket.TransferProduct(itemsToPlace, itemContainer.Positions);
                                itemContainer.ActivateItems(itemsToPlace);
                            }
                        }
                    }
                }
            }
            else if (playerInteraction.CurrentDraggable == null && !playerInteraction.PlayerTray.IsActive)
            {
                SoundPlayer.Instance.PlayButtonClick();
                FriersAssemblyBeginig?.Invoke();
                SetValueCollider(false);
                _cameraPositionChanger.ChangePosition(_cameraCurrentPosition);
            }
        }

        public void FillTable()
        {
            Debug.Log("Пробуем пополнить стол ингриидиентами");

            foreach (var fryerTool in _fryerTools)
            {
                if (!fryerTool.IsRaw)
                {
                    int valueFryerTool = fryerTool.GetCountActiveWellItems();

                    if (valueFryerTool > 0)
                    {
                        FryerContainer fryerContainer = GetCompatibleFryerContainer(fryerTool.ItemType);

                        if (fryerContainer != null)
                        {
                            int emptyContainerPosition = fryerContainer.GetInactiveValue();
                            Debug.Log("Пустых мест в контейнере " + emptyContainerPosition);
                            
                            /*if (emptyContainerPosition <= 0)
                                AttentionHintActivator.Instance.ShowHint(
                                    LocalizationManager.GetTermTranslation("No place"));*/
                            
                            int itemsToPlace = Mathf.Min(emptyContainerPosition, valueFryerTool);
                            Debug.Log("Меньшее число  " + itemsToPlace);

                            _transferItems.TransferJumpListItems(itemsToPlace, fryerTool.WellItems,
                                fryerContainer.Positions,
                                () =>
                                {
                                    Debug.Log("itemsToPlace ");
                                    fryerContainer.ActivateItems(itemsToPlace);
                                    fryerTool.ResetPosition();
                                    fryerTool.DeactivateWellItems(itemsToPlace);
                                });

                            // fryerContainer.ActivateItems(itemsToPlace);
                            // fryerTool.DeactivateWellItems(itemsToPlace);
                        }
                    }
                }
            }
        }

        public FryerContainer GetCompatibleFryerContainer(ItemType itemType)
        {
            foreach (var fryerTool in _fryerContainers)
            {
                if (fryerTool.ItemType == itemType)
                    return fryerTool;
            }

            return null;
        }

        public void SetValueCollider(bool value)
        {
            _collider.enabled = value;
            _assemblyFromDeepFry.enabled = !value;

            foreach (var containerCollidder in _containerColliders)
            {
                containerCollidder.enabled = !value;
                Debug.Log("value " + !value);
            }
        }
    }
}