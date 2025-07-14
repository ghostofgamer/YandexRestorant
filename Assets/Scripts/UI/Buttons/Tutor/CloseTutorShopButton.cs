using Enums;
using TutorialContent;
using UnityEngine;

namespace UI.Buttons.Tutor
{
    public class CloseTutorShopButton : AbstractButton
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private GameObject _touchCloseButton;
    
        public override void OnClick()
        {
            if (_tutorial.CurrentType == TutorialType.LetsSetPrice)
            {
                _tutorial.SetCurrentTutorialStage(TutorialType.LetsSetPrice);
                _touchCloseButton.SetActive(false);
                enabled = false;
            }
        }
    }
}