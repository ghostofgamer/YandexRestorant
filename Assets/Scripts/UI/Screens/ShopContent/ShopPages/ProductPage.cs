using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages
{
    public class ProductPage : ShopPage
    {
        [SerializeField] private GameObject _halfBackgroundImage;
        [SerializeField] private GameObject _fullBackgroundImage;

        public override void Open(int index)
        {
            base.Open(index);
            OpenScroll(index);
            _halfBackgroundImage.SetActive(index == 0);
            _fullBackgroundImage.SetActive(index != 0);
        }
    }
}