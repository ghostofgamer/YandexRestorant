using ADSContent;
using EnergyContent;
using Io.AppMetrica;
using UnityEngine;

namespace UI.Buttons
{
    public class FreeEnergyADButton : AbstractButton
    {
        [SerializeField] private Energy _energy;
        [SerializeField] private ADS _ads;
        
        public override void OnClick()
        {
            _ads.ShowRewarded(() =>
            {
                AppMetrica.ReportEvent("RewardAD", "{\"" + "FreeEnergyAD" + "\":null}");
                _energy.IncreaseEnergy(3);
            });
        }
    }
}
