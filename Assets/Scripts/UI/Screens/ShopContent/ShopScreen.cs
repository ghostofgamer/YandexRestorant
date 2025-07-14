using Enums;
using PlayerContent.LevelContent;
using TutorialContent;
using UI.Buttons.PageShopButtons;
using UI.Screens.ShopContent.ShopPages;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.ShopContent
{
    public class ShopScreen : AbstractScreen
    {
        [SerializeField] private Button[] _tutorDeactivateButton;
        [SerializeField] private ScrollRect _pageScroolTutor;
        [SerializeField] private ShopTutorialChanger _shopTutorialChanger;
        [SerializeField] private Button _tutorWorkPageButton;
        [SerializeField] private GameObject _touchShopImage;
        
        [SerializeField] private PageShopButton[] _pageShopButtons;
        [SerializeField] private ShopPage[] _shopPages;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private Button _closeButton;

        public override void OpenScreen()
        {
            base.OpenScreen();
            ActivateShopButton(0);
            OpenPage(0);
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }

        public virtual void OpenPage(int index)
        {
            if (_tutorial.CurrentType == TutorialType.LetsSetPrice)
            {
                DeactivateShopPages();
                ActivateShopButton(index);
                _shopPages[1].Open(0);
            }
            else
            {
                DeactivateShopPages();
                ActivateShopButton(index);
                _shopPages[index].Open(0);
            }
        }

        private void ActivateShopButton(int index)
        {
            if (_tutorial.CurrentType == TutorialType.OrderBurgerPatties)
            {
                SetInteractableButton(false);
                _closeButton.interactable = false;

                foreach (var tButton in _tutorDeactivateButton)
                    tButton.interactable = false;

                _shopTutorialChanger.ActivateRawCutlet();
            }
            else if (_tutorial.CurrentType == TutorialType.LetsSetPrice)
            {
                _closeButton.interactable = false;
                _touchShopImage.SetActive(false);
                _shopTutorialChanger.SetValueAddBurgerToMenuButton(false);
                
                foreach (var tButton in _pageShopButtons)
                    tButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                _closeButton.interactable = true;

                foreach (var tButton in _tutorDeactivateButton)
                    tButton.interactable = true;

                SetInteractableButton(true);
                DeactivateShopButtons();
                _pageShopButtons[index].ActivateButton();
            }
        }

        private void DeactivateShopButtons()
        {
            foreach (var pageShopButton in _pageShopButtons)
                pageShopButton.DeactivateButton();
        }

        private void DeactivateShopPages()
        {
            foreach (var screen in _shopPages)
                screen.Close();
        }

        public void MakePurchase()
        {
            _playerLevel.AddExp(5);
        }

        public void OpenMenuScreen()
        {
            base.OpenScreen();
            ActivateShopButton(0);
            OpenPage(1);
        }

        private void SetInteractableButton(bool value)
        {
            foreach (var pageShopButton in _pageShopButtons)
                pageShopButton.GetComponent<Button>().interactable = value;
        }
    }
}