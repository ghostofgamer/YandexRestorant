using System;
using UI;
using UnityEngine;

namespace EnergyContent
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] private FlyValue _flyValue;
        
        public int EnergyValue { get; private set; }

        public event Action<int> EnergyValueChanged;

        private void Start()
        {
            EnergyValue = PlayerPrefs.GetInt("EnergyValue", 10);
            SaveEnergy();
            EnergyValueChanged?.Invoke(EnergyValue);
        }

        public void IncreaseEnergy(int value)
        {
            if (value <= 0)
                return;
            
            _flyValue.ShowFly(value);
            EnergyValue += value;
            SaveEnergy();
            EnergyValueChanged?.Invoke(EnergyValue);
        }

        public void DecreaseEnergy(int value)
        {
            _flyValue.ShowFly(-value);
            EnergyValue -= value;
            SaveEnergy();
            EnergyValueChanged?.Invoke(EnergyValue);
        }

        private void SaveEnergy()
        {
            PlayerPrefs.SetInt("EnergyValue", EnergyValue);
        }
    }
}