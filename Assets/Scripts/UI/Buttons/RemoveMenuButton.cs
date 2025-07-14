using SettingsContent.SoundContent;
using UI.MenuUIContent;
using UnityEngine;

namespace UI.Buttons
{
    public class RemoveMenuButton : AbstractButton
    {
        [SerializeField] private MenuUIItem _menuUIItem;
        
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _menuUIItem.RemoveItemToMenu();
        }
    }
}