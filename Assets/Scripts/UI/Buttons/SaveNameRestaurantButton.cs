using TutorialContent;
using UnityEngine;

namespace UI.Buttons
{
    public class SaveNameRestaurantButton : AbstractButton
    {
        [SerializeField] private NameRestaurant _nameRestaurant;
        
        public override void OnClick()
        {
            _nameRestaurant.Save();
        }
    }
}
