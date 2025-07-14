using ADSContent;
using Io.AppMetrica;
using UnityEngine;
using WalletContent;

namespace UI.Buttons
{
    public class FreeMoneyADButton : AbstractButton
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private ADS _ads;

        public override void OnClick()
        {
            _ads.ShowRewarded(() =>
            {
                AppMetrica.ReportEvent("RewardAD", "{\"" + "FreeMoneyAD" + "\":null}");
                _wallet.Add(new DollarValue(10, 0));
            });
        }
    }
}