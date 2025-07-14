using PlayerContent.LevelContent;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class PlacesScrollContent : PageScrollContent
    {
        [SerializeField] private PlaceUIProduct[] _placeUIProducts;
        [SerializeField] private PlayerLevel _playerLevel;
        
        public override void Init()
        {
            foreach (var placeUIProduct in _placeUIProducts)
                placeUIProduct.Init();
            
            Debug.Log("GasmeObj " + gameObject.name);
        }
    }
}
