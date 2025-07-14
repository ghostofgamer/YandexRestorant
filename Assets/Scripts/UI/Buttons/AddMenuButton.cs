using SettingsContent.SoundContent;
using UI.MenuUIContent;
using UnityEngine;

namespace UI.Buttons
{
    public class AddMenuButton :AbstractButton
    {
        [SerializeField] private DishesUIItem _dishesUIItem;
    
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _dishesUIItem.AddItemToMenu();
        }
    }
}