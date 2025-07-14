using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using ItemContent;
using PlayerContent.LevelContent;
using RestaurantContent;
using RestaurantContent.TrayContent;
using SettingsContent.SoundContent;
using SoContent.AssemblyBurger;
using UI.Screens;
using UI.Screens.EquipmentContent;
using UnityEngine;

namespace KitchenEquipmentContent.AssemblyTables.CoffeeTableContent
{
    public class CoffeeAssemblyTable : AssemblyDrinkTable
    {
        [SerializeField] private GameObject _emptyCup;
        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;
        [SerializeField] private AssemblyBurgerItemConfig _assemblyBurgerItemConfig;
        [SerializeField] private CoffeeCounter _coffeeCounter;
        [SerializeField] private List<Transform> _wellPositions;
        [SerializeField] private Restaurant _restaurant;
        [SerializeField] private FullnessCoffeeCounter _fullnessCoffeeCounter;
        [SerializeField] private EquipmentUIProduct _equipmentUIProduct;
        [SerializeField] private PlayerLevel _playerLevel;

        private Coroutine _coroutine;
        private bool _isWorking = false;

        private void Start()
        {
            int value = PlayerPrefs.GetInt("CoffeeWellCups", 0);

            Debug.Log("CoffeeWellCups " + value);

            if (value > 0)
                LoadWellCups(value);
            
            gameObject.SetActive(_equipmentUIProduct.IsBuyed());
        }

        public void PourCoffee()
        {
            if (_isWorking || ItemContainer.GetActiveItemsValue() <= 0 || _fullnessCoffeeCounter.CurrentFullness < 10)
                return;

            Debug.Log("КОФЕ");
            _isWorking = true;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Pour());
        }

        private IEnumerator Pour()
        {
            Transform availablePosition = _wellPositions.FirstOrDefault(position => position.childCount == 0);

            if (availablePosition != null)
            {
                ItemContainer.DeactivateItems(1);
                _fullnessCoffeeCounter.UseCoffee();
                _emptyCup.SetActive(true);
                SoundPlayer.Instance.PlayPourDrink();
                yield return new WaitForSeconds(1f);
                _emptyCup.SetActive(false);
                Item coffeeInstance = _burgerIngridientSpawner.SpawnItem(ItemType.Coffee);
                coffeeInstance.SetParenContainer(_burgerIngridientSpawner.transform);
                coffeeInstance.gameObject.SetActive(true);
                coffeeInstance.transform.position = _emptyCup.transform.position;
                coffeeInstance.transform.rotation = Quaternion.identity;
                coffeeInstance.transform.localScale = _assemblyBurgerItemConfig.GetScale(ItemType.Coffee);

                _playerLevel.AddExp(5);

                Sequence sequence = DOTween.Sequence();

                if (_restaurant.TryGetTrayDrinkOrder(ItemType.Coffee, out Tray tray))
                {
                    _restaurant.SetDrinkOrder(tray, coffeeInstance);

                    Debug.Log("TRUE");
                    Transform position = tray.GetFirstAvailablePosition();

                    sequence.Append(coffeeInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(coffeeInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(coffeeInstance.transform.DOMove(position.position, 1f)
                        .SetEase(Ease.InOutQuad));

                    coffeeInstance.transform.SetParent(position);

                    sequence.Join(coffeeInstance.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => tray.TryCompletedOrder());
                }
                else
                {
                    Debug.Log("FALSE");

                    sequence.Append(coffeeInstance.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(coffeeInstance.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
                    sequence.Append(coffeeInstance.transform.DOMove(availablePosition.position, 0.5f)
                        .SetEase(Ease.InOutQuad));

                    coffeeInstance.transform.SetParent(availablePosition);

                    sequence.Join(coffeeInstance.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => _coffeeCounter.AddCoffee(coffeeInstance));
                }
            }
            else
            {
                Debug.Log("нету пустых позиций");
            }
            
            yield return new WaitForSeconds(1f);
            _isWorking = false;
        }

        private void LoadWellCups(int value)
        {
            StartCoroutine(StartLoad(value));
        }

        private IEnumerator StartLoad(int value)
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < value; i++)
            {
                Transform availablePosition = _wellPositions.FirstOrDefault(position => position.childCount == 0);
                Item coffeeInstance = _burgerIngridientSpawner.SpawnItem(ItemType.Coffee);
                coffeeInstance.transform.localScale = _assemblyBurgerItemConfig.GetScale(ItemType.Coffee);
                coffeeInstance.gameObject.SetActive(true);
                coffeeInstance.transform.SetParent(availablePosition);
                coffeeInstance.transform.position = availablePosition.position;
                _coffeeCounter.AddCoffee(coffeeInstance);
            }
        }


        public override void FillDrinkMachine(ItemDrinkPackage itemDrinkPackage)
        {
            if (itemDrinkPackage.CurrentFullness > 0)
            {
                _fullnessCoffeeCounter.RefillCoffee(itemDrinkPackage);
            }
        }
    }
}