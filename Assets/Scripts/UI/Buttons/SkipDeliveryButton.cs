using ADSContent;
using DeliveryContent;
using EnergyContent;
using Io.AppMetrica;
using SettingsContent.SoundContent;
using UnityEngine;

namespace UI.Buttons
{
    public class SkipDeliveryButton : AbstractButton
    {
        [SerializeField] private Delivery _delivery;
        [SerializeField] private bool _isAdButton;
        [SerializeField] private bool _isEnergyButton;
        [SerializeField] private ADS _ads;
        [SerializeField] private SkipCounter _skipCounter;
        [SerializeField] private Energy _energy;

        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();

            if (_isAdButton)
            {
                Debug.Log("РЕВАРД КНОПКА");
                _ads.ShowRewarded(() => _delivery.SpawnAllItems());
                AppMetrica.ReportEvent("RewardAD", "{\"" + "SkipDeliveryADS" + "\":null}");
            }
            else if (_isEnergyButton)
            {
                Debug.Log("ЕНЕРГИЯ КНОПКА КНОПКА КНОПКА");
                Debug.Log("_energy.EnergyValue" + _energy.EnergyValue);
                Debug.Log("_delivery.AmountDeliveries" + _delivery.AmountDeliveries);

                if (_energy.EnergyValue < _delivery.AmountDeliveries)
                    return;

                _energy.DecreaseEnergy(_delivery.AmountDeliveries);
                AppMetrica.ReportEvent("SkipDeliveryEnergy");
                _delivery.SpawnAllItems();
            }
            else
            {
                Debug.Log("КНОПКА КНОПКА КНОПКА КНОПКА");
                AppMetrica.ReportEvent("SkipDeliveryFreeButton");
                _delivery.SpawnAllItems();
                _skipCounter.SkipFirstActivate();
            }
        }
    }
}