using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using ItemContent;
using KitchenEquipmentContent.AssemblyTables.SodaTableContent;
using PlayerContent.LevelContent;
using RestaurantContent;
using RestaurantContent.TrayContent;
using SaveContent;
using SettingsContent.SoundContent;
using SoContent.AssemblyBurger;
using UI.Screens;
using UI.Screens.EquipmentContent;
using UnityEngine;

namespace KitchenEquipmentContent
{
    public class SodaAssemblyTable : AssemblyDrinkTable
    {
        [SerializeField] private GameObject[] _emptyCups;
        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;
        [SerializeField] private SodaCounter _sodaCounter;
        [SerializeField] private AssemblyBurgerItemConfig _assemblyBurgerItemConfig;
        [SerializeField] private PlayerLevel _playerLevel;
        
        // [SerializeField] private CoffeeCounter _coffeeCounter;
        [SerializeField] private List<Transform> _wellPositions;
        [SerializeField] private Restaurant _restaurant;
        [SerializeField] private SodaFullnessCounter[] _sodaFullnessCounters;
        [SerializeField] private EquipmentUIProduct _equipmentUIProduct;
        [SerializeField] private SodaSaver _sodaSaver;
        
        private Coroutine _coroutine;
        private bool _isWorking = false;

        private void Start()
        {
            List<ItemType> itemTypes = _sodaSaver.LoadItemTypesFromIndices();
            
            if (itemTypes.Count > 0)
                LoadWellCups(itemTypes.Count,itemTypes);
            
            gameObject.SetActive(_equipmentUIProduct.IsBuyed());
        }
        
        public void PourSoda(ItemType itemType, int index)
        {
            Debug.Log("НАЛИВАЕМ лимонад " + itemType);
            
            if (_isWorking || ItemContainer.GetActiveItemsValue() <= 0)
                return;
            
            SodaFullnessCounter sodaFullnessCounter = GetSodaFullnessCounter(itemType);
            int value = sodaFullnessCounter.CurrentFullness;

            if (value < 10)
            {
                Debug.Log("Соды мало тут  " + value);
                return;
            }

            Debug.Log("Сода  " + itemType);
            _isWorking = true;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Pour(itemType, index, sodaFullnessCounter));
        }

        private IEnumerator Pour(ItemType itemType, int index, SodaFullnessCounter sodaFullnessCounter)
        {
            Transform availablePosition = _wellPositions.FirstOrDefault(position => position.childCount == 0);

            if (availablePosition != null)
            {
                ItemContainer.DeactivateItems(1);
                _emptyCups[index].SetActive(true);
                sodaFullnessCounter.UseSoda();
                SoundPlayer.Instance.PlayPourDrink();
                yield return new WaitForSeconds(1f);
                _emptyCups[index].SetActive(false);
                Item sodaInstance = _burgerIngridientSpawner.SpawnItem(itemType);
                sodaInstance.SetParenContainer(_burgerIngridientSpawner.transform);
                sodaInstance.gameObject.SetActive(true);
                sodaInstance.transform.position = _emptyCups[index].transform.position;
                sodaInstance.transform.rotation = Quaternion.identity;
                sodaInstance.transform.localScale = _assemblyBurgerItemConfig.GetScale(itemType);
                
                _playerLevel.AddExp(5);
                
                Sequence sequence = DOTween.Sequence();

                if (_restaurant.TryGetTrayDrinkOrder(itemType, out Tray tray))
                {
                    _restaurant.SetSodaOrder(tray, sodaInstance);

                    Debug.Log("TRUE");
                    Transform position = tray.GetFirstAvailablePosition();

                    sequence.Append(sodaInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(sodaInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(sodaInstance.transform.DOMove(position.position, 1f)
                        .SetEase(Ease.InOutQuad));

                    sodaInstance.transform.SetParent(position);

                    sequence.Join(sodaInstance.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => tray.TryCompletedOrder());
                }
                else
                {
                    Debug.Log("FALSE");

                    sequence.Append(sodaInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(sodaInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(sodaInstance.transform.DOMove(availablePosition.position, 0.5f)
                        .SetEase(Ease.InOutQuad));

                    sodaInstance.transform.SetParent(availablePosition);

                    sequence.Join(sodaInstance.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => _sodaCounter.AddSoda(sodaInstance));
                }
            }
            else
            {
                if (_restaurant.TryGetTrayDrinkOrder(itemType, out Tray tray))
                {
                    Debug.Log("Позиций нету но есть заказ же ");
                    Transform position = tray.GetFirstAvailablePosition();
                    
                    ItemContainer.DeactivateItems(1);
                    _emptyCups[index].SetActive(true);
                    sodaFullnessCounter.UseSoda();
                    SoundPlayer.Instance.PlayPourDrink();
                    yield return new WaitForSeconds(1f);
                    _emptyCups[index].SetActive(false);
                    Item sodaInstance = _burgerIngridientSpawner.SpawnItem(itemType);
                    sodaInstance.SetParenContainer(_burgerIngridientSpawner.transform);
                    sodaInstance.gameObject.SetActive(true);
                    sodaInstance.transform.position = _emptyCups[index].transform.position;
                    sodaInstance.transform.rotation = Quaternion.identity;
                    sodaInstance.transform.localScale = _assemblyBurgerItemConfig.GetScale(itemType);
                    
                    _playerLevel.AddExp(5);
                    
                    _restaurant.SetSodaOrder(tray, sodaInstance);
                    
                    Sequence sequence = DOTween.Sequence();
                    
                    sequence.Append(sodaInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(sodaInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(sodaInstance.transform.DOMove(position.position, 1f)
                        .SetEase(Ease.InOutQuad));

                    sodaInstance.transform.SetParent(position);

                    sequence.Join(sodaInstance.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => tray.TryCompletedOrder());
                }
                
                Debug.Log("нету пустых позиций");
            }
            
            yield return new WaitForSeconds(1f);
            _isWorking = false;
        }

        public override void FillDrinkMachine(ItemDrinkPackage itemDrinkPackage)
        {
            if (itemDrinkPackage.CurrentFullness > 0)
            {
                SodaFullnessCounter sodaFullnessCounter = GetSodaFullnessCounter(itemDrinkPackage.ItemType);
                sodaFullnessCounter.RefillSoda(itemDrinkPackage);
                // _fullnessCoffeeCounter.RefillCoffee(itemDrinkPackage);
            }
        }

        public SodaFullnessCounter GetSodaFullnessCounter(ItemType itemType)
        {
            return _sodaFullnessCounters.FirstOrDefault(counter => counter.ItemType == itemType);
        }
        
        private void LoadWellCups(int value,List<ItemType> itemType)
        {
            StartCoroutine(StartLoad(value,itemType));
        }

        private IEnumerator StartLoad(int value,List<ItemType> itemType)
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < value; i++)
            {
                Transform availablePosition = _wellPositions.FirstOrDefault(position => position.childCount == 0);
                Item sodaInstance = _burgerIngridientSpawner.SpawnItem(itemType[i]);
                sodaInstance.gameObject.SetActive(true);
                sodaInstance.transform.SetParent(availablePosition);
                sodaInstance.transform.localScale = _assemblyBurgerItemConfig.GetScale(itemType[i]);
                sodaInstance.transform.position = availablePosition.position;
                _sodaCounter.AddSoda(sodaInstance);
            }
        }
    }
}