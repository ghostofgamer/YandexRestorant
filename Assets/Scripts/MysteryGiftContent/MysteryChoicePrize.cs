using System.Collections.Generic;
using ADSContent;
using DeliveryContent;
using Enums;
using Io.AppMetrica;
using PlayerContent.LevelContent;
using SettingsContent.SoundContent;
using UI.Screens;
using UnityEngine;
using WalletContent;
using Random = UnityEngine.Random;

namespace MysteryGiftContent
{
    public class MysteryChoicePrize : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private List<MysteryPrize> prizes = new List<MysteryPrize>();
        [SerializeField] private MysteryGift _mysteryGift;
        [SerializeField] private ADS _ads;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Delivery _delivery;
        [SerializeField] private MysteryBoxScreen _mysteryBoxScreen;
        [SerializeField] private CongratulationMysteryBoxScreen _congratulationMysteryBoxScreen;

        private MysteryPrize _randomPrize;

        private void OnEnable()
        {
            _mysteryGift.BoxActivation += SelectRandomPrize;
        }

        private void OnDisable()
        {
            _mysteryGift.BoxActivation -= SelectRandomPrize;
        }

        public void GivePrizeADS()
        {
            /*if (_randomPrize != null)
                _ads.ShowRewarded(() =>
                {
                    SoundPlayer.Instance.PlayMysteryBoxPrize();
                    AppMetrica.ReportEvent("RewardAD", "{\"" + "MysteryPrize" + "\":null}");
                    GetPrize(_randomPrize);
                    _mysteryBoxScreen.CloseScreen();
                    _congratulationMysteryBoxScreen.OpenScreen();
                    _congratulationMysteryBoxScreen.Init(_randomPrize.SpriteIcon, _randomPrize.Value);
                });*/
        }

        private void SelectRandomPrize()
        {
            List<MysteryPrize> eligiblePrizes = new List<MysteryPrize>();

            foreach (MysteryPrize prize in prizes)
            {
                if (prize.Level <= _playerLevel.CurrentLevel)
                    eligiblePrizes.Add(prize);
            }

            if (eligiblePrizes.Count > 0)
            {
                _randomPrize = eligiblePrizes[Random.Range(0, eligiblePrizes.Count)];
                Debug.Log("Вы выиграли: " + _randomPrize.MysteryPrizeType);
            }
            else
            {
                Debug.Log("Нет доступных призов для вашего уровня.");
            }
        }

        private void GetPrize(MysteryPrize mysteryPrize)
        {
            switch (mysteryPrize.MysteryPrizeType)
            {
                case MysteryPrizeType.Money:
                    _wallet.Add(new DollarValue(mysteryPrize.Value, 00));
                    break;

                case MysteryPrizeType.Bun:
                    _delivery.SpawnPrize(ItemType.Bun, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.Cutlet:
                    _delivery.SpawnPrize(ItemType.RawCutlet, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.PackageBurgers:
                    _delivery.SpawnPrize(ItemType.PackageBurgerPaper, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.Cheese:
                    _delivery.SpawnPrize(ItemType.Cheese, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.Tomato:
                    _delivery.SpawnPrize(ItemType.Tomato, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.Onion:
                    _delivery.SpawnPrize(ItemType.Onion, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.Salad:
                    _delivery.SpawnPrize(ItemType.Cabbage, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.Nuggets:
                    _delivery.SpawnPrize(ItemType.Nuggets, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.FrenchFries:
                    _delivery.SpawnPrize(ItemType.FrenchFries, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.PackageNuggets:
                    _delivery.SpawnPrize(ItemType.NuggetsPackage, mysteryPrize.Value);
                    break;

                case MysteryPrizeType.PackageFries:
                    _delivery.SpawnPrize(ItemType.FrenchFriesPackage, mysteryPrize.Value);
                    break;
            }
        }
    }

    [System.Serializable]
    public class MysteryPrize
    {
        public MysteryPrizeType MysteryPrizeType;
        public int Level;
        public int Value;
        public Sprite SpriteIcon;
    }
}