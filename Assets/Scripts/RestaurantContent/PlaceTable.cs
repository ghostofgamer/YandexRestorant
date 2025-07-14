using UI.Screens.ShopContent;
using UnityEngine;

namespace RestaurantContent
{
    public class PlaceTable : MonoBehaviour
    {
        [SerializeField] private PlaceUIProduct _placeUIProduct;

        private void Start()
        {
            Activate();
        }

        public void Activate()
        {
            gameObject.SetActive(_placeUIProduct.IsBuyed());
        }
    }
}