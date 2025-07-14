using KitchenEquipmentContent.AssemblyTables.CoffeeTableContent;
using UnityEngine;

namespace SaveContent
{
    [RequireComponent(typeof(FullnessCoffeeCounter))]
    public class CoffeeSaver : MonoBehaviour
    {
        [SerializeField] private CoffeeCounter _coffeeCounter;

        private FullnessCoffeeCounter _fullnessCoffeeCounter;
        private int _coffeeFullnessValue;

        private void Awake()
        {
            _fullnessCoffeeCounter = GetComponent<FullnessCoffeeCounter>();
        }

        private void OnEnable()
        {
            _fullnessCoffeeCounter.FullnessCoffeeChanged += SaveFullnessCoffee;
            _coffeeCounter.CoffeeItemsValueChanged += SaveWellCoffee;
        }

        private void OnDisable()
        {
            _fullnessCoffeeCounter.FullnessCoffeeChanged -= SaveFullnessCoffee;
            _coffeeCounter.CoffeeItemsValueChanged -= SaveWellCoffee;
        }

        private void SaveFullnessCoffee(int value)
        {
            PlayerPrefs.SetInt("CoffeeFullness", value);
        }

        private void SaveWellCoffee(int value)
        {
            PlayerPrefs.SetInt("CoffeeWellCups", value);
        }
    }
}