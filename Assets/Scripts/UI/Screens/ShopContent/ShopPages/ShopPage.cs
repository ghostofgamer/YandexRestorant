using UI.Buttons.PageShopButtons;
using UI.Screens.ShopContent.ShopPages.PageContents;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages
{
    public class ShopPage : MonoBehaviour
    {
        [SerializeField] private PageButton[] _pageButtons;
        [SerializeField] private PageScrollContent[] _pageContents;

        public virtual void Open(int index)
        {
            gameObject.SetActive(true);
            DeactivatePages();
            ActivatePage(index);
            OpenScroll(index);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void DeactivatePages()
        {
            foreach (var pageShopButton in _pageButtons)
                pageShopButton.DeactivateButton();

            foreach (var page in _pageContents)
                page.CloseScreen();
        }

        public void ActivatePage(int index)
        {
            DeactivatePages();
            _pageButtons[index].ActivateButton();
            _pageContents[index].OpenScreen();
        }
        
        protected void OpenScroll(int index)
        {
            Debug.Log("OpenScroll");
            _pageContents[index].Init();
        }
    }
}