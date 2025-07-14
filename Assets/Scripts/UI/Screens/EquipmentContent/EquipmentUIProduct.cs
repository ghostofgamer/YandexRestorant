using Enums;
using I2.Loc;
using Io.AppMetrica;
using SettingsContent;
using SettingsContent.SoundContent;
using TMPro;
using UI.Screens.ShopContent;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace UI.Screens.EquipmentContent
{
    public class EquipmentUIProduct : MonoBehaviour
    {
        public const string Equipment = "Equipment";

        [SerializeField] protected GameObject _ownedObjectInfo;
        [SerializeField] protected GameObject _requaredObjectInfo;
        [SerializeField] protected GameObject _buyObjectInfo;
        [SerializeField] private int _levelOpened;
        [SerializeField] private GameObject _equipment;
        [SerializeField] protected TMP_Text _requaredText;
        [SerializeField] protected TMP_Text _priceText;
        [SerializeField] private int _dollarPrice;
        [SerializeField] private int _centPrice;
        [SerializeField] protected ShopScreen _shopScreen;
        [SerializeField] protected Wallet _wallet;
        [SerializeField] private Color _activeButtonColor;
        [SerializeField] private Color _notActiveButtonColor;
        [SerializeField] private Image _buyButtonImage;
        [SerializeField] private bool _dependsOnZone;
        [SerializeField] private ZoneUIProduct _zoneUIProduct;
        [SerializeField] protected EquipmentType _equipmentType;
        [SerializeField] protected LanguageChanger _languageChanger;

        protected bool IsOwned;
        protected DollarValue CurrentPrice;

        private void OnEnable()
        {
            _languageChanger.LanguageChanged += ChangeLocalization;
        }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= ChangeLocalization;
        }

        public virtual void Init(int levelPlayer)
        {
            IsOwned = IsBuyed();
            CurrentPrice = new DollarValue(_dollarPrice, _centPrice);

            Initialization(levelPlayer);
            _buyButtonImage.color = _wallet.DollarValue.ToTotalCents() >= CurrentPrice.ToTotalCents()
                ? _activeButtonColor
                : _notActiveButtonColor;
        }

        public virtual void Buy()
        {
            if (_wallet.DollarValue.ToTotalCents() < CurrentPrice.ToTotalCents())
            {
                SoundPlayer.Instance.PlayError();
                Debug.Log("Не хватает денег ");
                return;
            }


            AppMetrica.ReportEvent("Equipment", "{\"" + _equipmentType.ToString() + "\":null}");
            SoundPlayer.Instance.PlayPayment();
            _wallet.Subtract(CurrentPrice);
            _shopScreen.MakePurchase();
            IsOwned = true;
            _ownedObjectInfo.SetActive(true);
            _buyObjectInfo.SetActive(false);
            _equipment.gameObject.SetActive(true);
            PlayerPrefs.SetInt(Equipment + _levelOpened, 1);
            _shopScreen.CloseScreen();
        }

        public virtual bool IsBuyed()
        {
            return PlayerPrefs.GetInt(Equipment + _levelOpened, 0) > 0;
        }

        public virtual void Initialization(int levelPlayer)
        {
            if (_dependsOnZone)
            {
                _requaredText.text = LocalizationManager.GetTermTranslation("Required kitchen zone");
                _priceText.text = $"{CurrentPrice.ToString()} ";

                if (_zoneUIProduct != null)
                {
                    _requaredObjectInfo.SetActive(!_zoneUIProduct.IsBuyed() && !IsOwned);
                    _ownedObjectInfo.SetActive(_zoneUIProduct.IsBuyed() && IsOwned);
                    _buyObjectInfo.SetActive(_zoneUIProduct.IsBuyed() && !IsOwned);
                }
            }
            else
            {
                _requaredText.text = $"{LocalizationManager.GetTermTranslation("Required")} {_levelOpened}";
                _priceText.text = $"{CurrentPrice.ToString()} ";
                _requaredObjectInfo.SetActive(levelPlayer < _levelOpened && !IsOwned);
                _ownedObjectInfo.SetActive(levelPlayer >= _levelOpened && IsOwned);
                _buyObjectInfo.SetActive(levelPlayer >= _levelOpened && !IsOwned);
            }
        }

        private void ChangeLocalization()
        {
            _requaredText.text = _dependsOnZone
                ? LocalizationManager.GetTermTranslation("Required kitchen zone")
                : _requaredText.text = $"{LocalizationManager.GetTermTranslation("Required")} {_levelOpened}";
        }
    }
}