using ADSContent;
using PlayerContent.LevelContent;
using UI.Buttons;
using UnityEngine;

public class RewardEXP : AbstractButton
{
    [SerializeField] private PlayerLevel _playerLevel;
    [SerializeField] private ADS _ads;

    public override void OnClick()
    {
        _ads.ShowRewarded(() => _playerLevel.AddExp(135));
    }
}