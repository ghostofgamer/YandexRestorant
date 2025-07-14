using System;
using Enums;
using I2.Loc;
using RestaurantContent;
using SettingsContent;
using SettingsContent.SoundContent;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace UI.Screens.ShopContent
{
    public class ZoneUIProduct : MonoBehaviour
    {
        [SerializeField] private GameObject _ownedObjectInfo;
        [SerializeField] private GameObject _requaredObjectInfo;
        [SerializeField] private GameObject _buyObjectInfo;
        [SerializeField] private int _levelOpened;
        [SerializeField] private GameObject _wallZone;
        [SerializeField] private ZoneWall _zoneWall;
        [SerializeField] private ZoneUIProduct _previousWallZone;
        [SerializeField] private TMP_Text _requaredText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private int _dollars;
        [SerializeField] private int _cents;
        [SerializeField] private ShopScreen _shopScreen;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Color _activeButtonColor;
        [SerializeField] private Color _notActiveButtonColor;
        [SerializeField] private Image _buyButtonImage;
        [SerializeField] private ZoneType _zoneType;
        [SerializeField] private LanguageChanger _languageChanger;
        
        private DollarValue _dollarValue;
        
        public bool IsOwned { get; private set; }

        private void OnEnable()
        {
            _languageChanger.LanguageChanged += ChangeLocalization;
        }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= ChangeLocalization;
        }
        
        public void Init(int levelPlayer)
        {
            IsOwned = IsBuyed();
            _dollarValue = new DollarValue(_dollars, _cents);
            ChangeLocalization();
            
            _priceText.text = $"{_dollarValue.ToString()} ";

            if (_previousWallZone == null)
            {
                SetValue(levelPlayer < _levelOpened && !IsOwned,
                    levelPlayer >= _levelOpened && IsOwned,
                    levelPlayer >= _levelOpened && !IsOwned);
            }
            else
            {
                SetValue((!IsOwned && !_previousWallZone.IsOwned) || levelPlayer < _levelOpened,
                    levelPlayer >= _levelOpened && IsOwned && _previousWallZone.IsOwned,
                    levelPlayer >= _levelOpened && !IsOwned && _previousWallZone.IsOwned);
            }

            _buyButtonImage.color = _wallet.DollarValue.ToTotalCents() >= _dollarValue.ToTotalCents()
                ? _activeButtonColor
                : _notActiveButtonColor;
        }

        public bool IsBuyed()
        {
            return PlayerPrefs.GetInt("Zona" + _zoneType, 0) > 0;
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
            PlayerPrefs.SetInt("Zona" + _zoneType, 1);
            _shopScreen.CloseScreen();
            _zoneWall.Activate();
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
        
        private void ChangeLocalization()
        {
            _requaredText.text = _previousWallZone == null
                ?$"{LocalizationManager.GetTermTranslation("Required")} {_levelOpened}"
                : $"{LocalizationManager.GetTermTranslation("Required")} {_levelOpened} {LocalizationManager.GetTermTranslation("prev zone")}";
        }
    }
}