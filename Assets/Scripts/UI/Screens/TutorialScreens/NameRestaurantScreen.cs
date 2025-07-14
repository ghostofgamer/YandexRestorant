using TMPro;
using UnityEngine;

namespace UI.Screens.TutorialScreens
{
    public class NameRestaurantScreen : AbstractScreen
    {
        [SerializeField] private TMP_Text _description;

        private void Start()
        {
            _description.text = "Congratulations on buying <color=yellow>your own restaurant!</color> \nWhat would you call it?";
        }
    }
}