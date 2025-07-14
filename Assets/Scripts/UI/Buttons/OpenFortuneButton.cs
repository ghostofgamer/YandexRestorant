using FortuneContent;
using SettingsContent.SoundContent;
using UnityEngine;

namespace UI.Buttons
{
    public class OpenFortuneButton : AbstractButton
    {
        [SerializeField] private Fortune _fortune;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick(); 
            _fortune.OnShow();
        }
    }
}