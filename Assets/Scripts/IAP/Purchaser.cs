using System;
using ADSContent;
using DeliveryContent;
using EnergyContent;
using Enums;
using MirraGames.SDK;
using RestaurantContent;
using SoContent;
using UI.Screens.AdsScreens;
using UnityEngine;
using WalletContent;

namespace IAP
{
    public class Purchaser : MonoBehaviour
    {
        [SerializeField] private UIInfo _uiInfo;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private InterstitialTimer _interstitialTimer;
        [SerializeField] private Delivery _delivery;
        [SerializeField] private RemoveAdScreen _removeAdScreen;
        [SerializeField] private StarterPackScreen _starterPackScreen;
        [SerializeField] private StoragePackScreen _storagePackScreen;
        [SerializeField] private GameObject _starterPackButton;
        [SerializeField] private GameObject _storagePackButton;
        [SerializeField] private Energy _energy;
        [SerializeField] private ADS _ads;
        [SerializeField] private ZoneWall _storageZoneWall;
        [SerializeField] private ShelfConfigs _shelfConfigs;
        [SerializeField] private GameObject[] _shelfes;


        public void ClickPurchaser(PurchaseType purchaseType)
        {
            MirraSDK.Payments.Purchase(
                productTag: purchaseType.ToString(),
                onSuccess: () =>
                {
                    Debug.Log("Товар успешно куплен");
                    // Выдать товар игроку
                    OnPurchaseCompleted(purchaseType);
                },
                onError: () => Debug.Log("Товар не был куплен")
            );
        }

        public void OnPurchaseCompleted(PurchaseType purchaseType)
        {
            switch (purchaseType)
            {
                case PurchaseType.Money100:
                    AddMoney(100);
                    break;

                case PurchaseType.Money1100:
                    AddMoney(1100);
                    break;

                case PurchaseType.Money2750:
                    AddMoney(2750);
                    break;

                case PurchaseType.Money8000:
                    AddMoney(8000);
                    break;

                case PurchaseType.Money500:
                    AddMoney(500);
                    break;

                case PurchaseType.Money20000:
                    AddMoney(20000);
                    break;

                case PurchaseType.Energy30:
                    AddEnergy(30);
                    break;

                case PurchaseType.Energy150:
                    AddEnergy(150);
                    break;

                case PurchaseType.Energy450:
                    AddEnergy(450);
                    break;

                case PurchaseType.Energy1850:
                    AddEnergy(1850);
                    break;

                case PurchaseType.Energy5000:
                    AddEnergy(5000);
                    break;
                
                case PurchaseType.RemoveAds:
                    RemoveAds();
                    break;
                
                case PurchaseType.StarterPack:
                    StarterPack();
                    break;
                
                case PurchaseType.StoragePack:
                    PayStoragePack();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(purchaseType), purchaseType, null);
            }


            /*switch (product.definition.id)
            {
                case "com.serbull.iaptutorial.money100":
                    AddMoney(100);
                    break;

                case "com.serbull.iaptutorial.removeads":
                    RemoveAds();
                    break;

                case "com.serbull.iaptutorial.money500":
                    AddMoney(500);
                    break;

                case "com.serbull.iaptutorial.money1100":
                    AddMoney(1100);
                    break;

                case "com.serbull.iaptutorial.money2750":
                    AddMoney(2750);
                    break;

                case "com.serbull.iaptutorial.money8000":
                    AddMoney(8000);
                    break;

                case "com.serbull.iaptutorial.money20000":
                    AddMoney(20000);
                    break;

                case "com.serbull.iaptutorial.starterpack":
                    StarterPack();
                    break;

                case "com.serbull.iaptutorial.energy30":
                    AddEnergy(30);
                    break;

                case "com.serbull.iaptutorial.energy150":
                    AddEnergy(150);
                    break;

                case "com.serbull.iaptutorial.energy450":
                    AddEnergy(450);
                    break;

                case "com.serbull.iaptutorial.energy1850":
                    AddEnergy(1850);
                    break;

                case "com.serbull.iaptutorial.energy5000":
                    AddEnergy(5000);
                    break;

                case "com.serbull.iaptutorial.storagepack":
                    PayStoragePack();
                    break;
            }*/
        }

        private void RemoveAds()
        {
            PlayerPrefs.SetInt("removeADS", 1);
            Debug.Log("On Purchase RemoveAds Completed");
            // AppMetrica.ReportEvent("In_App", "{\"" + "RemoveADS" + "\":null}");

            if (_interstitialTimer != null)
                _interstitialTimer.SetValue(false);

            if (_ads != null)
                _ads.SetValue(false);

            if (_uiInfo != null)
                _uiInfo.UpdateRemoveAdsButton();

            if (_removeAdScreen != null)
                _removeAdScreen.CloseScreen();
        }

        private void StarterPack()
        {
            PlayerPrefs.SetInt("StarterPack", 1);
            AddMoney(150);
            // AppMetrica.ReportEvent("In_App", "{\"" + "StarterPack" + "\":null}");
            _delivery.SpawnPrize(ItemType.Bun, 3);
            _delivery.SpawnPrize(ItemType.RawCutlet, 3);

            Debug.Log("On Purchase StarterPack Completed");

            if (_starterPackScreen != null)
                _starterPackScreen.CloseScreen();

            if (_starterPackButton != null)
                _starterPackButton.SetActive(false);
        }

        private void AddMoney(int value)
        {
            _wallet.Add(new DollarValue(value, 0));
            Debug.Log("On Purchase AddMoney Completed");
        }

        private void AddEnergy(int value)
        {
            _energy.IncreaseEnergy(value);
            Debug.Log("On Purchase AddEnergy Completed");
        }

        public void PayStoragePack()
        {
            PlayerPrefs.SetInt("StoragePack", 1);
            // AppMetrica.ReportEvent("In_App", "{\"" + "StoragePack" + "\":null}");
            AddMoney(300);

            _delivery.SpawnPrize(ItemType.Bun, 4);
            _delivery.SpawnPrize(ItemType.RawCutlet, 4);
            _delivery.SpawnPrize(ItemType.PackageBurgerPaper, 4);
            _delivery.SpawnPrize(ItemType.Cheese, 4);
            _delivery.SpawnPrize(ItemType.Coffee, 4);
            _delivery.SpawnPrize(ItemType.CupCoffeeEmpty, 4);
            _delivery.SpawnPrize(ItemType.Tomato, 4);

            int activeShelfs = 0;

            foreach (var shelf in _shelfes)
            {
                if (shelf.activeSelf)
                    activeShelfs++;
            }

            DollarValue amountPrice = new DollarValue(0, 0);

            for (int i = 0; i < activeShelfs; i++)
            {
                amountPrice += _shelfConfigs.shelves[i].price;
                Debug.Log("@ PlusPrice " + _shelfConfigs.shelves[i].price);
            }

            foreach (var shelf in _shelfes)
                shelf.SetActive(true);

            PlayerPrefs.SetInt("ShelfBuyed" + EquipmentType.Shelf, _shelfConfigs.shelves.Length - 1);

            if (PlayerPrefs.GetInt("Zona" + ZoneType.Storage, 0) > 0)
            {
                amountPrice += new DollarValue(100, 0);
            }
            else
            {
                PlayerPrefs.SetInt("Zona" + ZoneType.Storage, 1);
                _storageZoneWall.Activate();
            }

            Debug.Log("@ amountPrice " + amountPrice);

            AddMoney(amountPrice.Dollars);

            if (_storagePackScreen != null)
                _storagePackScreen.CloseScreen();

            if (_storagePackButton != null)
                _storagePackButton.SetActive(false);
        }
    }
}