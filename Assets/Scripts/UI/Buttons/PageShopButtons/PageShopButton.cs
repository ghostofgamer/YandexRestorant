using SettingsContent.SoundContent;
using UI.Screens;
using UI.Screens.ShopContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.PageShopButtons
{
    public class PageShopButton : AbstractButton
    {
        [SerializeField] private int _index;
        [SerializeField] private Color _activeButtonColor;
        [SerializeField] private Color _notActiveButtonColor;
        [SerializeField] private Image _imageButton;
        [SerializeField] private ShopScreen _shopScreen;

        protected int Index => _index;

        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _shopScreen.OpenPage(_index);
        }

        public void ActivateButton()
        {
            ChangeColorButton(_activeButtonColor);
        }

        public void DeactivateButton()
        {
            ChangeColorButton(_notActiveButtonColor);
        }

        private void ChangeColorButton(Color color)
        {
            _imageButton.color = color;
        }
    }
}