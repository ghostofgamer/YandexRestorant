using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AttentionHintContent;
using DG.Tweening;
using Enums;
using I2.Loc;
using InteractableContent;
using PlayerContent.LevelContent;
using RestaurantContent;
using RestaurantContent.TrayContent;
using UnityEngine;
using UnityEngine.Serialization;

namespace KitchenEquipmentContent.FryerContent
{
    public class AssemblyFromDeepFry : MonoBehaviour
    {
        [SerializeField] private AssemblyFryerTable _assemblyFryerTable;
        [SerializeField] private FryerContainer[] _fryerContainers;
        [SerializeField] private Transform[] _itemWellPositions;
        [SerializeField] private Transform _centerPos;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private DeepFryerItemCounter _deepFryerItemCounter;
        [SerializeField] private Restaurant _restaurant;
        [SerializeField] private TransferItems _transferItems;

        [FormerlySerializedAs("_burgerPrefabPairs")] [SerializeField]
        private List<ItemPrefabPair> _itemPrefabPairs = new List<ItemPrefabPair>();

        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;

        private Camera _camera;
        private bool _isCreated = false;
        private Coroutine _pauseCoroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_isCreated)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    // ItemContainer selectedContainer = hit.collider.GetComponent<ItemContainer>();
                    TriggerContainer selectedContainer = hit.collider.GetComponent<TriggerContainer>();

                    if (selectedContainer != null)
                    {
                        Debug.Log("selectedContainer нашелся");

                        ItemType itemType = selectedContainer.ItemType;

                        int valuePackage = selectedContainer.ItemContainer.GetActiveItemsValue();
                        Debug.Log("активных упаковок : " + valuePackage);

                        if (valuePackage <= 0)
                        {
                            Debug.Log("не хватате упаковок : ");
                            
                            AttentionHintActivator.Instance.ShowHint(
                                LocalizationManager.GetTermTranslation("No packaging"));
                            
                            return;
                        }

                        foreach (var _fryerContainer in _fryerContainers)
                        {
                            if (_fryerContainer.ItemType == itemType)
                                Create(_fryerContainer, selectedContainer.ItemContainer, itemType);
                        }
                    }
                    else
                    {
                        Debug.Log("selectedContainer NULL");
                    }
                }
            }
        }

        private void Create(FryerContainer fryerContainer, ItemContainer itemContainer, ItemType itemType)
        {
            int activeItemContainers = fryerContainer.GetActiveValue();

            if (activeItemContainers <= 0)
                return;

            GameObject itemPrefab = _itemPrefabPairs.FirstOrDefault(pair => pair.Type == itemType)?.Prefab;

            if (itemPrefab == null)
                return;

            Debug.Log("Создали ");
            Transform availablePosition = _itemWellPositions.FirstOrDefault(position => position.childCount == 0);

            if (availablePosition != null)
            {
                Debug.Log("1");
                Item itemInstance = _burgerIngridientSpawner.SpawnItem(itemType);
                itemInstance.SetParenContainer(_burgerIngridientSpawner.transform);
                itemInstance.gameObject.SetActive(true);
                itemInstance.transform.position = _centerPos.position;
                itemInstance.transform.rotation = Quaternion.identity;
                StartCreatePause();
                itemContainer.DeactivateItems(1);
                fryerContainer.DeactivateItems(1);
                _playerLevel.AddExp(5);
                // _deepFryerItemCounter.AddItem(itemInstance);

                if (_restaurant.TryGetTrayExtraOrder(itemType, out Tray tray))
                {
                    Debug.Log("3");
                    _restaurant.SetExtraOrder(tray, itemInstance);
                    Transform position = tray.GetFirstAvailablePosition();

                    _transferItems.TransferToTray(itemInstance.gameObject, position, () =>
                    {
                        _assemblyFryerTable.FillTable();
                        tray.TryCompletedOrder();
                    });
                }
                else
                {
                    Debug.Log("5");
                    _transferItems.TransferToTray(itemInstance.gameObject, availablePosition, () =>
                    {
                        _assemblyFryerTable.FillTable();
                        _deepFryerItemCounter.AddItem(itemInstance);
                    });
                }
            }
            else
            {
                Debug.Log("6");
                if (_restaurant.TryGetTrayExtraOrder(itemType, out Tray tray))
                {
                    Debug.Log("10");
                    Item itemInstance = _burgerIngridientSpawner.SpawnItem(itemType);
                    itemInstance.SetParenContainer(_burgerIngridientSpawner.transform);
                    itemInstance.gameObject.SetActive(true);
                    itemInstance.transform.position = _centerPos.position;
                    itemInstance.transform.rotation = Quaternion.identity;
                    StartCreatePause();
                    itemContainer.DeactivateItems(1);
                    fryerContainer.DeactivateItems(1);

                    _restaurant.SetExtraOrder(tray, itemInstance);
                    _playerLevel.AddExp(5);

                    Transform position = tray.GetFirstAvailablePosition();

                    _transferItems.TransferToTray(itemInstance.gameObject, position, () =>
                    {
                        _assemblyFryerTable.FillTable();
                        tray.TryCompletedOrder();
                    });
                }
            }
        }

        public void SimpleCreatItem(ItemType itemType)
        {
            Transform availablePosition = _itemWellPositions.FirstOrDefault(position => position.childCount == 0);
            Item itemInstance = _burgerIngridientSpawner.SpawnItem(itemType);
            itemInstance.gameObject.SetActive(true);
            itemInstance.transform.SetParent(availablePosition);
            itemInstance.transform.position = availablePosition.position;
            itemInstance.transform.localRotation = Quaternion.Euler(0, 0, 0);
            itemInstance.transform.localScale = Vector3.one;
            _deepFryerItemCounter.AddItem(itemInstance);
        }

        private void StartCreatePause()
        {
            if (_pauseCoroutine != null)
                StopCoroutine(_pauseCoroutine);

            _pauseCoroutine = StartCoroutine(CreatePause());
        }

        private IEnumerator CreatePause()
        {
            _isCreated = true;
            yield return _waitForSeconds;
            _isCreated = false;
        }
    }
}