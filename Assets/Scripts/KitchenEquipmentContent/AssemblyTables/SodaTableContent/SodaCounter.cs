using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using RestaurantContent;
using RestaurantContent.TrayContent;
using UnityEngine;

namespace KitchenEquipmentContent.AssemblyTables.SodaTableContent
{
    public class SodaCounter : MonoBehaviour
    {
        [SerializeField] private Restaurant _restaurant;

        private List<Item> _sodas = new List<Item>();

        public event Action<List<Item>> SodaItemsValueChanged;

        public void AddSoda(Item item)
        {
            _sodas.Add(item);
            SodaItemsValueChanged?.Invoke(_sodas);
        }

        public void RemoveSoda(Item item)
        {
            _sodas.Remove(item);
            SodaItemsValueChanged?.Invoke(_sodas);
        }

        public void CheckWaitNeedSoda(ItemType itemType)
        {
            if (TryFindNeedSoda(itemType, out Item burger))
            {
                Sequence sequence = DOTween.Sequence();

                if (_restaurant.TryGetTrayDrinkOrder(itemType, out Tray tray))
                {
                    _restaurant.SetSodaOrder(tray, burger);
                    
                    Transform position = tray.GetFirstAvailablePosition();

                    sequence.Append(burger.transform.DOMove(position.position, 1f)
                        .SetEase(Ease.InOutQuad));

                    burger.transform.SetParent(position);

                    sequence.Join(burger.transform
                            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                            .SetEase(Ease.Linear))
                        .OnComplete(() => tray.TryCompletedOrder());
                }
            }
            else
            {
                // Debug.Log("FALSE Автоматическое " + itemType);
            }
        }

        public bool TryFindNeedSoda(ItemType itemType, out Item burger)
        {
            burger = _sodas.FirstOrDefault(b => b.ItemType == itemType);
            return burger != null;
        }
    }
}