using System;
using System.Collections;
using System.Linq;
using AttentionHintContent;
using DG.Tweening;
using Enums;
using I2.Loc;
using InteractableContent;
using PlayerContent;
using SettingsContent.SoundContent;
using SoContent.AssemblyBurger;
using TMPro;
using TutorialContent;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenEquipmentContent
{
    public class Grill : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Item[] _rawCutletItems;
        [SerializeField] private Item[] _readyCutletItems;
        [SerializeField] private ItemType _currentType;
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider _boxCollider;
        [SerializeField] private GameObject _progressFryUI;
        [SerializeField] private BurgerIngridientSpawner _burgerIngridientSpawner;
        [SerializeField] private AssemblyBurgerItemConfig _assemblyBurgerItemConfig;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private GameObject _fryEffect;

        public TMP_Text grillText;
        public Image fillImage;
        public float grillTime = 3f;
        private bool _isClosed;

        public event Action<int, int> ValueActiveItemsChanged;

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }

        private void Start()
        {
            foreach (var rawCutlet in _rawCutletItems)
                rawCutlet.gameObject.SetActive(false);

            foreach (var readyCutlet in _readyCutletItems)
                readyCutlet.gameObject.SetActive(false);

            int rawValue = PlayerPrefs.GetInt("RawCutletGrill", 0);
            int readyValue = PlayerPrefs.GetInt("WellCutletGrill", 0);

            if (rawValue > 0)
            {
                _currentType = ItemType.RawCutlet;

                for (int i = 0; i < rawValue; i++)
                    _rawCutletItems[i].gameObject.SetActive(true);
            }

            if (readyValue > 0)
            {
                _currentType = ItemType.Cutlet;

                for (int i = 0; i < readyValue; i++)
                    _readyCutletItems[i].gameObject.SetActive(true);
            }
        }

        public void Action(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.CurrentDraggable != null)
                return;

            if (_isClosed)
            {
                OpenGrill();
                return;
            }

            ItemType itemType = playerInteraction.PlayerTray.CurrentType;
            Item[] item = GetItemsByType(itemType);

            if (_currentType == ItemType.Cutlet)
            {
                if (playerInteraction.PlayerTray.CurrentType == ItemType.Cutlet)
                {
                    Debug.Log("1");
                    int activePos = playerInteraction.PlayerTray.GetActivePositionValue(ItemType.Cutlet);
                    int activeCount = CountNotActiveItems(_readyCutletItems);
                    int itemsToPlace = Mathf.Min(activeCount, activePos);
                    
                    if (activePos <= 0)
                        AttentionHintActivator.Instance.ShowHint(
                            LocalizationManager.GetTermTranslation("The tray is empty"));
                    else if (activeCount <= 0)
                        AttentionHintActivator.Instance.ShowHint(LocalizationManager.GetTermTranslation("No place"));
                    
                    

                    if (itemsToPlace > 0)
                    {
                        playerInteraction.PlayerTray.PutAway(ItemType.Cutlet, itemsToPlace);

                        int completedAnimations = 0;
                        Vector3 scale = _assemblyBurgerItemConfig.GetScale(ItemType.Cutlet);

                        for (int i = 0; i < itemsToPlace; i++)
                        {
                            Debug.Log("ТУТА!!!!");

                            Item newItem = _burgerIngridientSpawner.SpawnItem(ItemType.Cutlet);
                            newItem.gameObject.SetActive(true);
                            newItem.transform.position = playerInteraction.PlayerTray.Positions[i].position;
                            newItem.transform.localScale = scale;

                            Sequence sequence = DOTween.Sequence();
                            sequence.Append(newItem.transform
                                .DOMove(_readyCutletItems[i].transform.position, 0.15f)
                                .SetEase(Ease.InOutQuad));
                            sequence.Join(newItem.transform
                                .DORotate(_readyCutletItems[i].transform.eulerAngles, 0.15f)
                                .SetEase(Ease.InOutQuad));
                            sequence.OnComplete(() =>
                            {
                                completedAnimations++;
                                newItem.transform.position = Vector3.zero;
                                newItem.gameObject.SetActive(false);

                                if (completedAnimations == itemsToPlace)
                                {
                                    ActivateItems(_readyCutletItems, itemsToPlace);
                                }
                            });
                        }

                        // ActivateItems(_readyCutletItems, itemsToPlace);
                    }
                }
                else if (playerInteraction.PlayerTray.CurrentType == ItemType.Empty)
                {
                    Debug.Log("3");
                    int emptyPos = playerInteraction.PlayerTray.GetEmptyPositionValue(ItemType.Cutlet);
                    int activeCount = CountActiveItems(_readyCutletItems);
                    int itemsToPlace = Mathf.Min(activeCount, emptyPos);

                    if (itemsToPlace > 0)
                    {
                        DeactivateItems(_readyCutletItems, itemsToPlace);

                        int completedAnimations = 0;
                        Vector3 scale = _assemblyBurgerItemConfig.GetScale(ItemType.Cutlet);

                        for (int i = 0; i < itemsToPlace; i++)
                        {
                            Item newItem = _burgerIngridientSpawner.SpawnItem(ItemType.Cutlet);
                            newItem.gameObject.SetActive(true);
                            newItem.transform.position = _readyCutletItems[i].transform.position;
                            newItem.transform.localScale = scale;

                            Sequence sequence = DOTween.Sequence();
                            sequence.Append(newItem.transform
                                .DOMove(playerInteraction.PlayerTray.transform.position, 0.15f)
                                .SetEase(Ease.InOutQuad));
                            sequence.Join(newItem.transform
                                .DORotate(playerInteraction.PlayerTray.transform.eulerAngles, 0.15f)
                                .SetEase(Ease.InOutQuad));
                            sequence.OnComplete(() =>
                            {
                                completedAnimations++;
                                newItem.transform.position = Vector3.zero;
                                newItem.gameObject.SetActive(false);

                                if (completedAnimations == itemsToPlace)
                                {
                                    playerInteraction.PlayerTray.Put(ItemType.Cutlet, itemsToPlace);
                                }
                            });
                        }

                        // playerInteraction.PlayerTray.Put(ItemType.Cutlet, itemsToPlace);
                    }
                }
                else if (playerInteraction.PlayerTray.CurrentType == ItemType.RawCutlet)
                {
                    AttentionHintActivator.Instance.ShowHint(
                        LocalizationManager.GetTermTranslation(
                            "You can't put raw cutlets in: take away cooked cutlets"));
                }
            }
            else if (_currentType == ItemType.RawCutlet)
            {
                if (playerInteraction.PlayerTray.CurrentType == ItemType.RawCutlet)
                {
                    int emptyPositions =
                        playerInteraction.PlayerTray.GetActivePositionValue(playerInteraction.PlayerTray.CurrentType);
                    int inactiveCount = CountNotActiveItems(item);
                    int itemsToPlace = Mathf.Min(inactiveCount, emptyPositions);

                    if (emptyPositions <= 0)
                        AttentionHintActivator.Instance.ShowHint(
                            LocalizationManager.GetTermTranslation("The tray is empty"));
                    else if (inactiveCount <= 0)
                        AttentionHintActivator.Instance.ShowHint(LocalizationManager.GetTermTranslation("No place"));

                    Debug.Log("СТОЛ " + itemsToPlace);
                    Debug.Log("emptyPositions " + emptyPositions);
                    Debug.Log("activeCount " + inactiveCount);

                    if (itemsToPlace > 0)
                    {
                        if (_tutorial.CurrentType == TutorialType.PutCutletsOnGrill)
                        {
                            _tutorial.SetCurrentTutorialStage(TutorialType.PutCutletsOnGrill);
                        }

                        playerInteraction.PlayerTray.PutAway(ItemType.RawCutlet, itemsToPlace);

                        int completedAnimations = 0;
                        Vector3 scale = _assemblyBurgerItemConfig.GetScale(ItemType.RawCutlet);

                        for (int i = 0; i < itemsToPlace; i++)
                        {
                            Debug.Log("ТУТА!!!! " + playerInteraction.PlayerTray.CurrentType);

                            Item newItem = _burgerIngridientSpawner.SpawnItem(ItemType.RawCutlet);
                            newItem.gameObject.SetActive(true);
                            newItem.transform.position = playerInteraction.PlayerTray.Positions[i].position;
                            newItem.transform.localScale = scale;

                            Sequence sequence = DOTween.Sequence();
                            sequence.Append(newItem.transform
                                .DOMove(_readyCutletItems[i].transform.position, 0.15f)
                                .SetEase(Ease.InOutQuad));
                            sequence.Join(newItem.transform
                                .DORotate(_readyCutletItems[i].transform.eulerAngles, 0.15f)
                                .SetEase(Ease.InOutQuad));
                            sequence.OnComplete(() =>
                            {
                                completedAnimations++;
                                newItem.transform.position = Vector3.zero;
                                newItem.gameObject.SetActive(false);

                                if (completedAnimations == itemsToPlace)
                                {
                                    ActivateItems(item, itemsToPlace);
                                }
                            });
                        }

                        // ActivateItems(item, itemsToPlace);
                    }
                }
                else if (playerInteraction.PlayerTray.CurrentType == ItemType.Empty)
                {
                    FryCutlets();
                }
                else
                {
                    AttentionHintActivator.Instance.ShowHint("Нельзя положить готовые котлеты, когда на гриле сырые");
                    Debug.Log("Нельзя положить готовые котлеты, когда на гриле сырые.");
                }
            }
            else
            {
                if (playerInteraction.PlayerTray.CurrentType == ItemType.RawCutlet ||
                    playerInteraction.PlayerTray.CurrentType == ItemType.Cutlet)
                {
                    int inactiveCount = CountNotActiveItems(item);
                    Debug.Log("Гриль колличество не активных " + inactiveCount);
                    int emptyPositions =
                        playerInteraction.PlayerTray.GetActivePositionValue(playerInteraction.PlayerTray.CurrentType);
                    Debug.Log("TRA колличество сырых " + emptyPositions);
                    int itemsToPlace = Mathf.Min(inactiveCount, emptyPositions);
                    Debug.Log("СКОЛЬКО " + itemsToPlace);

                    if (itemsToPlace > 0)
                    {
                        _currentType = playerInteraction.PlayerTray.CurrentType;
                        playerInteraction.PlayerTray.PutAway(playerInteraction.PlayerTray.CurrentType, itemsToPlace);

                        int completedAnimations = 0;
                        Vector3 scale = _assemblyBurgerItemConfig.GetScale(playerInteraction.PlayerTray.CurrentType);

                        for (int i = 0; i < itemsToPlace; i++)
                        {
                            Debug.Log("ТУТА!!!! " + playerInteraction.PlayerTray.CurrentType);

                            if (_tutorial.CurrentType == TutorialType.PutCutletsOnGrill)
                            {
                                Debug.Log("МАЯЯЯЯЯЯЯЯЯЯЯЯЯЯЯК");
                                _tutorial.SetCurrentTutorialStage(TutorialType.PutCutletsOnGrill);
                            }

                            Item newItem = _burgerIngridientSpawner.SpawnItem(_currentType);
                            newItem.gameObject.SetActive(true);
                            newItem.transform.position = playerInteraction.PlayerTray.Positions[i].position;
                            newItem.transform.localScale = scale;

                            Sequence sequence = DOTween.Sequence();
                            sequence.Append(newItem.transform
                                .DOMove(_readyCutletItems[i].transform.position, 0.15f)
                                .SetEase(Ease.InOutQuad));
                            sequence.Join(newItem.transform
                                .DORotate(_readyCutletItems[i].transform.eulerAngles, 0.15f)
                                .SetEase(Ease.InOutQuad));
                            sequence.OnComplete(() =>
                            {
                                completedAnimations++;
                                newItem.transform.position = Vector3.zero;
                                newItem.gameObject.SetActive(false);

                                if (completedAnimations == itemsToPlace)
                                {
                                    ActivateItems(item, itemsToPlace);
                                }
                            });
                        }


                        // ActivateItems(item, itemsToPlace);
                    }
                }
                else
                {
                    AttentionHintActivator.Instance.ShowHint(LocalizationManager.GetTermTranslation("Nothing to cook"));
                    Debug.Log("Нельзя положить готовые котлеты на пустой гриль.");
                }
            }
        }

        private Item[] GetItemsByType(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.RawCutlet => _rawCutletItems,
                ItemType.Cutlet => _readyCutletItems,
                _ => null
            };
        }

        private void ActivateItems(Item[] items, int value)
        {
            if (items == null)
            {
                Debug.LogError("Items array is null.");
                return;
            }

            int activatedCount = 0;

            foreach (var item in items)
            {
                if (!item.gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                    activatedCount++;

                    if (activatedCount >= value)
                        break;
                }
            }

            int countRaw = _rawCutletItems.Count(item => item.gameObject.activeInHierarchy);
            int countWell = _readyCutletItems.Count(item => item.gameObject.activeInHierarchy);
            ValueActiveItemsChanged?.Invoke(countRaw, countWell);
        }

        private void DeactivateItems(Item[] items, int value)
        {
            if (items == null)
            {
                Debug.LogError("Items array is null.");
                return;
            }

            int deactivatedCount = 0;

            for (int i = items.Length - 1; i >= 0; i--)
            {
                if (items[i].gameObject.activeSelf)
                {
                    items[i].gameObject.SetActive(false);
                    deactivatedCount++;

                    if (deactivatedCount >= value)
                        break;
                }
            }

            if (items.All(item => !item.gameObject.activeSelf))
                _currentType = ItemType.Empty;

            int countRaw = _rawCutletItems.Count(item => item.gameObject.activeInHierarchy);
            int countWell = _readyCutletItems.Count(item => item.gameObject.activeInHierarchy);
            ValueActiveItemsChanged?.Invoke(countRaw, countWell);
        }

        private int CountActiveItems(Item[] items)
        {
            int count = 0;
            foreach (var item in items)
            {
                if (item.gameObject.activeSelf)
                {
                    count++;
                }
            }

            return count;
        }

        private int CountNotActiveItems(Item[] items)
        {
            int count = 0;

            foreach (var item in items)
            {
                if (!item.gameObject.activeSelf)
                {
                    count++;
                }
            }

            return count;
        }

        private void FryCutlets()
        {
            StartCoroutine(StartFryCutlets());
        }

        private IEnumerator StartFryCutlets()
        {
            _isClosed = true;
            _animator.SetBool("FryCutlet", true);
            _boxCollider.enabled = false;
            yield return new WaitForSeconds(1f);
            _fryEffect.SetActive(true);
            _audioSource.Play();
            _progressFryUI.SetActive(true);
            // grillText.text = "Grill <color=yellow>Raw</color>";
            grillText.text =
                $"{LocalizationManager.GetTermTranslation("Grill")} <color=yellow>{LocalizationManager.GetTermTranslation("Raw")}</color>";
            fillImage.fillAmount = 0f;

            float elapsedTime = 0f;
            while (elapsedTime < grillTime)
            {
                elapsedTime += Time.deltaTime;
                fillImage.fillAmount = elapsedTime / grillTime;
                yield return null;
            }

            if (_tutorial.CurrentType == TutorialType.FryCutletGrill)
            {
                _tutorial.SetCurrentTutorialStage(TutorialType.FryCutletGrill);
            }

            _fryEffect.SetActive(false);
            _audioSource.Stop();
            // grillText.text = "Grill <color=green>Medium</color>";
            grillText.text =
                $"{LocalizationManager.GetTermTranslation("Grill")} <color=green>{LocalizationManager.GetTermTranslation("Well")}</color>";
            SoundPlayer.Instance.PlayGrillWell();
            // _animator.SetBool("FryCutlet",false);

            int activeCount = CountActiveItems(_rawCutletItems);

            foreach (var rawItem in _rawCutletItems)
                rawItem.gameObject.SetActive(false);

            for (int i = 0; i < activeCount; i++)
                _readyCutletItems[i].gameObject.SetActive(true);

            ValueActiveItemsChanged?.Invoke(0, activeCount);

            _currentType = ItemType.Cutlet;
            _boxCollider.enabled = true;
        }

        private void OpenGrill()
        {
            _isClosed = false;
            _animator.SetBool("FryCutlet", false);
            _progressFryUI.SetActive(false);
        }
    }
}