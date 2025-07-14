using DisableInterContent;
using UnityEngine;

namespace UI.Buttons
{
    public class GetShowReward : AbstractButton
    {
        [SerializeField] private DisablerInter _disablerInter;
        
        public override void OnClick()
        {
            _disablerInter.GetReward();
        }
    }
}