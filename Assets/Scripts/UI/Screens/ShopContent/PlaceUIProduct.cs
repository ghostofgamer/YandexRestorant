using I2.Loc;
using RestaurantContent;
using SettingsContent.SoundContent;
using TMPro;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace UI.Screens.ShopContent
{
    public class PlaceUIProduct : MonoBehaviour
    {
        [SerializeField] private GameObject _ownedObjectInfo;
        [SerializeField] private GameObject _requaredObjectInfo;
        [SerializeField] private int _index;
        [SerializeField] private GameObject _buyObjectInfo;
        [SerializeField] private ZoneUIProduct _zoneProduct;
        [SerializeField] private TMP_Text _requaredText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private ShopScreen _shopScreen;
        [SerializeField] private int _dollars;
        [SerializeField] private int _cents;
        [SerializeField] private PlaceTable _placeTable;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Color _activeButtonColor;
        [SerializeField] private Color _notActiveButtonColor;
        [SerializeField] private Image _buyButtonImage;
        [SerializeField] private PlacesScrollContent _placesScrollContent;

        private DollarValue _dollarValue;

        public bool IsOwned { get; private set; }

        public void Init()
        {
            _dollarValue = new DollarValue(_dollars, _cents);
            IsOwned = IsBuyed();
            /*_requaredText.text = _zoneProduct != null
                ? LocalizationManager.GetTermTranslation("Buy Zone")
                : LocalizationManager.GetTermTranslation("Buy Zone");*/
            _priceText.text = $"{_dollarValue.ToString()} ";

            if (_zoneProduct == null)
            {
                SetValue(false, IsOwned, !IsOwned);
            }
            else
            {
                SetValue(!_zoneProduct.IsBuyed(),
                    _zoneProduct.IsBuyed() && IsOwned,
                    _zoneProduct.IsBuyed() && !IsOwned);
            }

            _buyButtonImage.color = _wallet.DollarValue.ToTotalCents() >= _dollarValue.ToTotalCents()
                ? _activeButtonColor
                : _notActiveButtonColor;
        }

        public bool IsBuyed()
        {
            return PlayerPrefs.GetInt("Place" + _index, 0) > 0;
        }

        public void Buy()
        {
            if (_wallet.DollarValue.ToTotalCents() < _dollarValue.ToTotalCents())
            {
                SoundPlayer.Instance.PlayError();
                Debug.Log("Не хватает денег ");
                return;
            }

            SoundPlayer.Instance.PlayPayment();
            _wallet.Subtract(_dollarValue);
            _shopScreen.MakePurchase();
            IsOwned = true;
            _ownedObjectInfo.SetActive(true);
            _buyObjectInfo.SetActive(false);
            // _wallZone.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Place" + _index, 1);
            _shopScreen.CloseScreen();
            _placeTable.Activate();
        }

        private void SetValue(bool requaredObjectValue, bool ownedObjectValue, bool buyObjectValue)
        {
            Debug.Log("requaredObjectValue " + requaredObjectValue);
            Debug.Log("ownedObjectValue " + ownedObjectValue);
            Debug.Log("buyObjectValue " + buyObjectValue);

            _requaredObjectInfo.SetActive(requaredObjectValue);
            _ownedObjectInfo.SetActive(ownedObjectValue);
            _buyObjectInfo.SetActive(buyObjectValue);
        }
    }
}