using System;
using ItemContent;
using SettingsContent.SoundContent;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenEquipmentContent.AssemblyTables.CoffeeTableContent
{
    public class FullnessCoffeeCounter : MonoBehaviour
    {
        [SerializeField] private Image _imageFullness;

        private int _maxFullness = 100;

        public event Action<int> FullnessCoffeeChanged;

        public int CurrentFullness { get; private set; }

        private void Start()
        {
            int value = PlayerPrefs.GetInt("CoffeeFullness", 0);
            CurrentFullness = value;
            UpdateFillAmount();
        }

        public void UseCoffee()
        {
            if (CurrentFullness >= 10)
            {
                CurrentFullness -= 10;
                FullnessCoffeeChanged?.Invoke(CurrentFullness);
                UpdateFillAmount();
            }
            else
            {
                Debug.Log("Недостаточно кофе в кофемашине.");
            }
        }

        public void RefillCoffee(ItemDrinkPackage itemDrinkPackage)
        {
            int neededCoffee = _maxFullness - CurrentFullness;
            int coffeeToAdd = Mathf.Min(neededCoffee, itemDrinkPackage.CurrentFullness);

            if (coffeeToAdd > 0)
                SoundPlayer.Instance.PlayRefillDrinksMachine();

            CurrentFullness += coffeeToAdd;
            FullnessCoffeeChanged?.Invoke(CurrentFullness);
            itemDrinkPackage.PourOut(coffeeToAdd);
            // coffeeInPacket -= coffeeToAdd;

            UpdateFillAmount();

            Debug.Log($"Кофемашина заполнена. Осталось в пачке: {itemDrinkPackage.CurrentFullness}");
        }

        private void UpdateFillAmount()
        {
            _imageFullness.fillAmount = (float)CurrentFullness / _maxFullness;
        }
    }
}