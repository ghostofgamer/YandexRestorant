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
        
        private const string CoffeeFullnessKey = "CoffeeFullness";
        private const string CoffeeWellCupsKey = "CoffeeWellCups";

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
            // PlayerPrefs.SetInt("CoffeeFullness", value);
            StorageHelper.SetInt(CoffeeFullnessKey, value);
        }

        private void SaveWellCoffee(int value)
        {
            StorageHelper.SetInt(CoffeeWellCupsKey, value);
            // PlayerPrefs.SetInt("CoffeeWellCups", value);
        }
    }
}