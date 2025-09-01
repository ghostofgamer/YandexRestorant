using System;
using LoadingSceneContent;
using MirraGames.SDK;
using UI;
using UnityEngine;

namespace EnergyContent
{
    public class Energy : MonoBehaviour
    {
        private const string EnergyKey = "EnergyValue";

        [SerializeField] private FlyValue _flyValue;
        [SerializeField] private LoadingGame _loadingGame;

        public int EnergyValue { get; private set; }

        public event Action<int> EnergyValueChanged;

        private void OnEnable()
        {
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            _loadingGame.MirraSDKInitialization -= Init;
        }

        /*private void Start()
        {
            /*EnergyValue = PlayerPrefs.GetInt("EnergyValue", 10);
            SaveEnergy();
            EnergyValueChanged?.Invoke(EnergyValue);#1#
        }*/

        private void Init()
        {
            EnergyValue = MirraSDK.Data.GetInt(EnergyKey, 10);
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
            MirraSDK.Data.SetInt(EnergyKey, EnergyValue, true);
            MirraSDK.Data.Save();

            // PlayerPrefs.SetInt("EnergyValue", EnergyValue);
        }
    }
}