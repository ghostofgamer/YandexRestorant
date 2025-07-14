using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using RestaurantContent;
using RestaurantContent.TrayContent;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class DeepFryerItemCounter : MonoBehaviour
    {
        [SerializeField] private Restaurant _restaurant;
        
        private List<Item> _items = new List<Item>();
        
        public event Action<List<Item>> ItemsValueChanged;
        
        public void AddItem(Item item)
        {
            _items.Add(item);
            ItemsValueChanged?.Invoke(_items);
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
            ItemsValueChanged?.Invoke(_items);
        }
        
        public void CheckWaitNeedBurgers(ItemType itemType)
        {
            if (TryFindNeedBurger(itemType, out Item extraItem))
            {
                Sequence sequence = DOTween.Sequence();

                if (_restaurant.TryGetTrayExtraOrder(itemType, out Tray tray))
                {
                    _restaurant.SetExtraOrder(tray, extraItem);
                    Transform position = tray.GetFirstAvailablePosition();

                    sequence.Append(extraItem.transform.DOMove(position.position, 1f)
                        .SetEase(Ease.InOutQuad));

                    extraItem.transform.SetParent(position);

                    sequence.Join(extraItem.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => tray.TryCompletedOrder());
                }
            }
            else
            {
            }
        }

        private bool TryFindNeedBurger(ItemType itemType, out Item extraItem)
        {
            extraItem = _items.FirstOrDefault(b => b.ItemType == itemType);
            return extraItem != null;
        }
    }
}