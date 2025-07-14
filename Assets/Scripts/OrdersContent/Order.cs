using System;
using Enums;

namespace OrdersContent
{
    [Serializable]
    public class Order
    {
        public event Action BurgerCompleted;
        public event Action DrinkCompleted;
        public event Action ExtraCompleted;
        public event Action OrderCompleted;
        
        public event Action ChangeOrder;

        public ItemType BurgerItemOrder { get; private set; }

        public ItemType DrinkItemOrder { get; private set; }

        public ItemType ExtraItemOrder { get; private set; }

        public int IndexTable { get; private set; }

        public bool IsBurgerCompleted { get; private set; }

        public bool IsDrinkCompleted { get; private set; }

        public bool IsExtraCompleted { get; private set; }

        public Order(ItemType burgerItemOrder, ItemType drinkItemOrder, ItemType extraItemOrder)
        {
            BurgerItemOrder = burgerItemOrder;
            DrinkItemOrder = drinkItemOrder;
            ExtraItemOrder = extraItemOrder;

            IsBurgerCompleted = burgerItemOrder == ItemType.Empty;
            IsDrinkCompleted = drinkItemOrder == ItemType.Empty;
            IsExtraCompleted = extraItemOrder == ItemType.Empty;
        }

        public void SetTable(int index)
        {
            IndexTable = index;
        }

        public void SetBurgerCompleted(bool isCompleted)
        {
            IsBurgerCompleted = isCompleted;
            ChangeOrder?.Invoke();
            // BurgerCompleted?.Invoke();
        }

        public void SetDrinkCompleted(bool isCompleted)
        {
            IsDrinkCompleted = isCompleted;
            // DrinkCompleted?.Invoke();
            ChangeOrder?.Invoke();
        }

        public void SetExtraCompleted(bool isCompleted)
        {
            IsExtraCompleted = isCompleted;
            ChangeOrder?.Invoke();
            // ExtraCompleted?.Invoke();
        }

        public bool IsOrderCompleted()
        {
            bool burgerCompleted = IsBurgerCompleted || BurgerItemOrder == ItemType.Empty;
            bool drinkCompleted = IsDrinkCompleted || DrinkItemOrder == ItemType.Empty;
            bool extraCompleted = IsExtraCompleted || ExtraItemOrder == ItemType.Empty;

            bool orderCompleted = burgerCompleted && drinkCompleted && extraCompleted;

            if (orderCompleted)
                OrderCompleted?.Invoke();

            return orderCompleted;
        }
    }
}