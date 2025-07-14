using System;
using System.Collections.Generic;
using AttentionHintContent;
using DG.Tweening;
using Enums;
using I2.Loc;
using InteractableContent;
using ItemContent;
using PlayerContent;
using SoContent;
using UnityEngine;

namespace ShelfContent
{
    public class Shelf : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Transform[] _positions;
        [SerializeField] private DeliveryConfig _deliveryConfig;
        [SerializeField] private BoxesCounter _boxesCounter;
        
        private List<ItemBasket> _itemBaskets = new List<ItemBasket>();
        private List<ItemDrinkPackage> _itemDrinkPackages = new List<ItemDrinkPackage>();

        public event Action<List<ItemBasket>, List<ItemDrinkPackage>> ListItemChanged;

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
                int freePosCount = GetFreePositionCount();
                Debug.Log(freePosCount);

                if (freePosCount <= 0)
                {
                    AttentionHintActivator.Instance.ShowHint(LocalizationManager.GetTermTranslation("No place"));
                    return;
                }

                ItemBasket basket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();
                Draggable draggable = playerInteraction.CurrentDraggable.GetComponent<Draggable>();
                ItemDrinkPackage drinkPackage = playerInteraction.CurrentDraggable.GetComponent<ItemDrinkPackage>();

                if (draggable != null)
                {
                    draggable.PutOnShelf();

                    Transform position = GetFreePosition();

                    Sequence sequence = DOTween.Sequence();

                    if (basket != null)
                    {
                        _itemBaskets.Add(basket);
                        basket.SetShelf(this);

                        _boxesCounter.RemoveBox(basket.gameObject);
                        
                        playerInteraction.PutItemShelf();
                        basket.transform.SetParent(position);

                        ListItemChanged?.Invoke(_itemBaskets, _itemDrinkPackages);

                        sequence.Append(basket.transform.DOMove(position.position, 0.3f)
                            .SetEase(Ease.InOutQuad));
                        sequence.Join(basket.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear));
                    }

                    if (drinkPackage != null)
                    {
                        _itemDrinkPackages.Add(drinkPackage);
                        drinkPackage.SetShelf(this);
                        playerInteraction.PutItemShelf();
                        drinkPackage.transform.SetParent(position);
                        
                        _boxesCounter.RemoveBox(drinkPackage.gameObject);
                        
                        ListItemChanged?.Invoke(_itemBaskets, _itemDrinkPackages);

                        sequence.Append(drinkPackage.transform.DOMove(position.position, 0.3f)
                            .SetEase(Ease.InOutQuad));
                        sequence.Join(drinkPackage.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear));
                    }
                }
                else
                {
                    Debug.Log("NULL");
                }
            }
        }

        public int GetFreePositionCount()
        {
            int freeCount = 0;

            foreach (var position in _positions)
            {
                if (position.childCount == 0)
                    freeCount++;
            }

            return freeCount;
        }

        public Transform GetFreePosition()
        {
            foreach (var position in _positions)
            {
                if (position.childCount == 0)
                    return position;
            }

            return null;
        }

        public void Remove(ItemBasket itemBasket)
        {
            _itemBaskets.Remove(itemBasket);
            
            _boxesCounter.AddBox(itemBasket.gameObject);
            
            ListItemChanged?.Invoke(_itemBaskets, _itemDrinkPackages);
        }

        public void RemoveDrinkPackage(ItemDrinkPackage drinkPackage)
        {
            _itemDrinkPackages.Remove(drinkPackage);
            
            _boxesCounter.AddBox(drinkPackage.gameObject);
            
            ListItemChanged?.Invoke(_itemBaskets, _itemDrinkPackages);
        }

        public void Initialization(List<ItemType> itemTypes)
        {
            foreach (var itemType in itemTypes)
            {
                Transform position = GetFreePosition();
                GameObject prefab = _deliveryConfig.GetPrefabByItemType(itemType);

                if (position == null)
                    return;

                if (prefab != null)
                {
                    GameObject instance = Instantiate(prefab, position.position, Quaternion.identity);
                    ItemBasket basket = instance.GetComponent<ItemBasket>();
                    ItemDrinkPackage drinkPackage = instance.GetComponent<ItemDrinkPackage>();
                    
                    if (basket != null)
                    {
                        _itemBaskets.Add(basket);
                        basket.SetShelf(this);
                        basket.transform.SetParent(position);
                        ListItemChanged?.Invoke(_itemBaskets, _itemDrinkPackages);
                        basket.GetComponent<Rigidbody>().isKinematic = true;
                        /*basket.transform.position = position.position;
                        basket.transform.rotation = quaternion.identity;*/
                    }

                    if (drinkPackage != null)
                    {
                        _itemDrinkPackages.Add(drinkPackage);
                        drinkPackage.SetShelf(this);
                        drinkPackage.transform.SetParent(position);
                        ListItemChanged?.Invoke(_itemBaskets, _itemDrinkPackages);
                        drinkPackage.GetComponent<Rigidbody>().isKinematic = true;
                        /*drinkPackage.transform.position = position.position;
                        drinkPackage.transform.rotation = quaternion.identity;*/
                    }
                }
            }
        }
    }
}