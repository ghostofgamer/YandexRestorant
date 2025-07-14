using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using RestaurantContent.TrayContent;
using UnityEngine;

namespace RestaurantContent
{
    public class BurgersCounter : MonoBehaviour
    {
        [SerializeField] private Restaurant _restaurant;
        
        private List<Item> _burgers = new List<Item>();
        public event Action<List<Item>> BurgerItemsValueChanged;

        public void AddBurger(Item item)
        {
            _burgers.Add(item);
            BurgerItemsValueChanged?.Invoke(_burgers);
        }

        public void RemoveBurger(Item item)
        {
            _burgers.Remove(item);
            BurgerItemsValueChanged?.Invoke(_burgers);
        }

        public void CheckWaitNeedBurgers(ItemType itemType)
        {
            if (TryFindNeedBurger(itemType, out Item burger))
            {
                Sequence sequence = DOTween.Sequence();

                if (_restaurant.TryGetTrayOrder(itemType, out Tray tray))
                {
                    _restaurant.SetBurgerOrder(tray, burger);
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
                Debug.Log("FALSE Автоматическое");
            }
        }

        public bool TryFindNeedBurger(ItemType itemType, out Item burger)
        {
            burger = _burgers.FirstOrDefault(b => b.ItemType == itemType);
            return burger != null;
        }
    }
}