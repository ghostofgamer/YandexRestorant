using ADSContent;
using UI.Buttons;
using UnityEngine;
using WalletContent;

public class RewardMoney : AbstractButton
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private ADS _ads;

    public override void OnClick()
    {
        _ads.ShowRewarded(() => _wallet.Add(new DollarValue(35, 16)));
    }
}