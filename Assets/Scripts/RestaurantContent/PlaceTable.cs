using LoadingSceneContent;
using UI.Screens.ShopContent;
using UnityEngine;

namespace RestaurantContent
{
    public class PlaceTable : MonoBehaviour
    {
        [SerializeField] private PlaceUIProduct _placeUIProduct;
        [SerializeField]private LoadingGame _loadingGame;

        private void OnEnable()
        {
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            _loadingGame.MirraSDKInitialization -= Init;
        }

        /*private void Start()
        {
            Activate();
        }*/

        private void Init()
        {
            Activate();
        }

        public void Activate()
        {
            gameObject.SetActive(_placeUIProduct.IsBuyed());
        }
    }
}