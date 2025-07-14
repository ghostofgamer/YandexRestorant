using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class RefuseGiftButton : AbstractButton
    {
        [SerializeField] private MysteryBoxScreen _mysteryBoxScreen;
        [SerializeField] private GameObject _dontRushScreen;
        
        public override void OnClick()
        {
            _dontRushScreen.gameObject.SetActive(false);
            _mysteryBoxScreen.CloseScreen();
        }
    }
}