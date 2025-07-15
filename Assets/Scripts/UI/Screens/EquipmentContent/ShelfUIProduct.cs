using I2.Loc;
using SettingsContent.SoundContent;
using SoContent;
using StorageContent;
using TMPro;
using UnityEngine;

namespace UI.Screens.EquipmentContent
{
    public class ShelfUIProduct : EquipmentUIProduct
    {
        [SerializeField] private ShelfConfigs _shelfConfigs;
        [SerializeField] private GameObject[] _shelf;
        [SerializeField] private TMP_Text _nameItem;
        [SerializeField] private GameObject _storage1;
        [SerializeField] private Storage _storage;

        private int _currentBuyShelfIndex = -1;

        private void OnEnable()
        {
            _languageChanger.LanguageChanged += ChangeLocalization;
        }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= ChangeLocalization;
        }

        public override void Init(int levelPlayer)
        {
            _currentBuyShelfIndex = PlayerPrefs.GetInt("ShelfBuyed" + _equipmentType, -1);
            Initialization(levelPlayer);
        }

        public override void Buy()
        {
            int nextShelfIndex = _currentBuyShelfIndex + 1;
            Debug.Log("nextShelfIndex " + nextShelfIndex);

            if (nextShelfIndex < _shelfConfigs.shelves.Length)
            {
                if (_wallet.DollarValue.ToTotalCents() < CurrentPrice.ToTotalCents())
                {
                    SoundPlayer.Instance.PlayError();
                    Debug.Log("Не хватает денег ");
                    return;
                }

                // AppMetrica.ReportEvent("Equipment", "{\"" + "Shelf" + "\":null}");
                SoundPlayer.Instance.PlayPayment();
                _wallet.Subtract(CurrentPrice);
                _shopScreen.MakePurchase();
                _currentBuyShelfIndex = nextShelfIndex;
                PlayerPrefs.SetInt("ShelfBuyed" + _equipmentType, _currentBuyShelfIndex);
                ActivateShelf(_currentBuyShelfIndex);
                _shopScreen.CloseScreen();
                Initialization(_currentBuyShelfIndex);
            }
            else
            {
                Debug.Log("Все шкафы уже куплены");
            }
        }

        public override void Initialization(int levelPlayer)
        {
            if (_currentBuyShelfIndex < 0)
            {
                // _nameItem.text = .name;
                _nameItem.text =
                    $"{LocalizationManager.GetTermTranslation("Shelf")} {_shelfConfigs.shelves[0].index + 1}";
                CurrentPrice = _shelfConfigs.shelves[0].price;
                _priceText.text = $"{CurrentPrice} ";
            }
            else if (_currentBuyShelfIndex + 1 < _shelfConfigs.shelves.Length)
            {
                // _nameItem.text = _shelfConfigs.shelves[_currentBuyShelfIndex + 1].name;
                _nameItem.text =
                    $"{LocalizationManager.GetTermTranslation("Shelf")} {_shelfConfigs.shelves[_currentBuyShelfIndex + 1].index + 1}";
                CurrentPrice = _shelfConfigs.shelves[_currentBuyShelfIndex + 1].price;
                _priceText.text = $"{CurrentPrice} ";

                if (_shelfConfigs.shelves[_currentBuyShelfIndex + 1].storage1ToUnlock)
                {
                    // _requaredObjectInfo.SetActive(!_storage1.activeSelf);
                    _requaredObjectInfo.SetActive(!_storage.IsOpened);
                    // _requaredText.text = $"Required  is storage 1";
                    _requaredText.text = $"{LocalizationManager.GetTermTranslation("StorageRequired")} 1";
                    // _buyObjectInfo.SetActive(_storage1.activeSelf);
                    _buyObjectInfo.SetActive(_storage.IsOpened);
                    Debug.Log("_storage1.activeSelf " + _storage.IsOpened);
                }
            }
            else
            {
                _ownedObjectInfo.SetActive(true);
                _buyObjectInfo.SetActive(false);
                _requaredObjectInfo.SetActive(false);
                // _nameItem.text = "Shelf";
                _nameItem.text = LocalizationManager.GetTermTranslation("Shelf");
                _priceText.text = "";
            }
        }

        private void ActivateShelf(int index)
        {
            _shelf[index].SetActive(true);
        }

        private void ChangeLocalization()
        {
            if (_currentBuyShelfIndex < 0)
            {
                _nameItem.text =
                    $"{LocalizationManager.GetTermTranslation("Shelf")} {_shelfConfigs.shelves[0].index + 1}";
            }
            else if (_currentBuyShelfIndex + 1 < _shelfConfigs.shelves.Length)
            {
                _nameItem.text =
                    $"{LocalizationManager.GetTermTranslation("Shelf")} {_shelfConfigs.shelves[_currentBuyShelfIndex + 1].index + 1}";

                if (_shelfConfigs.shelves[_currentBuyShelfIndex + 1].storage1ToUnlock)
                    _requaredText.text = $"{LocalizationManager.GetTermTranslation("StorageRequired")} 1";
            }
            else
            {
                _nameItem.text = LocalizationManager.GetTermTranslation("Shelf");
            }
        }

        /*public override bool IsBuyed()
        {

        }*/
    }
}