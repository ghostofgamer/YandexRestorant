using ADSContent;
using EnergyContent;
using Io.AppMetrica;
using UnityEngine;
using WalletContent;

namespace UI.Screens.NewLevelContent
{
    public class NewLevelUpScreen : AbstractScreen
    {
        [SerializeField] private GameObject _firstScreen;
        [SerializeField] private GameObject _secondScreen;
        [SerializeField] private ADS _ads;
        [SerializeField] private Energy _energy;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private GameObject[] _effects;
        [SerializeField] private NewLevelUpViewer _newLevelUpViewer;

        public override void OpenScreen()
        {
            _newLevelUpViewer.Init();
            DeactivateScreens();
            base.OpenScreen();
            _firstScreen.SetActive(true);
            SetActiveEffects(true);
        }

        public override void CloseScreen()
        {
            SetActiveEffects(false);
            base.CloseScreen();
        }

        public void ChooseDontX2()
        {
            OpenSecondScreen();
            AddPrize(2, 25);
        }

        public void ChooseRewardX2()
        {
            _ads.ShowRewarded(() =>
            {
                OpenSecondScreen();
                AppMetrica.ReportEvent("RewardAD", "{\"" + "ChooseRewardX2UpLevel" + "\":null}");
                AddPrize(4, 50);
            });
        }

        private void OpenSecondScreen()
        {
            _firstScreen.SetActive(false);
            _secondScreen.SetActive(true);
            _newLevelUpViewer.ShowRewardLeveling();
        }

        private void DeactivateScreens()
        {
            _firstScreen.SetActive(false);
            _secondScreen.SetActive(false);
        }

        private void AddPrize(int _energyValue, int _moneyValue)
        {
            _wallet.Add(new DollarValue(_moneyValue, 0));
            _energy.IncreaseEnergy(_energyValue);
        }

        private void SetActiveEffects(bool value)
        {
            foreach (var effect in _effects)
                effect.SetActive(value);
        }
    }
}