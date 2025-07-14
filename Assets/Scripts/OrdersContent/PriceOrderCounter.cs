using System;
using Enums;
using SoContent;
using UnityEngine;
using WalletContent;
using Random = System.Random;

namespace OrdersContent
{
    public class PriceOrderCounter : MonoBehaviour
    {
        private const string CurrentPriceKey = "CurrentPrice";
        private static readonly Random _random = new Random();
        
        [SerializeField] private ItemsConfig _itemsConfig;

        public DollarValue Price;

        public DollarValue GetPriceOrder(Order order)
        {
            int burgerTotalCents = 0;

            if (order.BurgerItemOrder != ItemType.Empty)
            {
                burgerTotalCents = PlayerPrefs.GetInt(CurrentPriceKey + order.BurgerItemOrder,
                    _itemsConfig.GetItemConfig(order.BurgerItemOrder).RecommendedPrice.ToTotalCents());
            }

            int drinkTotalCents = 0;

            if (order.DrinkItemOrder != ItemType.Empty)
            {
                drinkTotalCents = PlayerPrefs.GetInt(CurrentPriceKey + order.DrinkItemOrder,
                    _itemsConfig.GetItemConfig(order.DrinkItemOrder).RecommendedPrice.ToTotalCents());
            }

            int extraTotalCents = 0;

            if (order.ExtraItemOrder != ItemType.Empty)
            {
                extraTotalCents = PlayerPrefs.GetInt(CurrentPriceKey + order.ExtraItemOrder,
                    _itemsConfig.GetItemConfig(order.ExtraItemOrder).RecommendedPrice.ToTotalCents());
            }

            int totalCents = burgerTotalCents + drinkTotalCents + extraTotalCents;
            DollarValue price = new DollarValue(0, 0).FromTotalCents(totalCents);


            return price;
        }

        public DollarValue GetCash(DollarValue dollarValuePriceOrder)
        {
            int dollars = dollarValuePriceOrder.Dollars;
            
            int minThreshold = (dollars < 100) 
                ? Math.Max(10, (dollars / 10 + 1) * 10) 
                : 100;
            
            if (minThreshold > 100) 
                return dollarValuePriceOrder;
            
            int randomAmount = _random.Next(minThreshold / 10, 11) * 10;
    
            return new DollarValue(randomAmount, 0);
        }

        public DollarValue GetChange(DollarValue price, DollarValue cash)
        {
            int totalCents = cash.ToTotalCents() - price.ToTotalCents();
            DollarValue change = new DollarValue(0,0).FromTotalCents(totalCents);

            return change;
        }
    }
}