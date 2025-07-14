using System.Collections;
using Io.AppMetrica;
using SettingsContent.SoundContent;
using UI.Buttons;
using UnityEngine;
using WalletContent;

namespace ADSContent.Popups
{
    public class AddMoneyAdButton : AbstractButton
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private ADS _ads;
        [SerializeField] private PopUpAdActivator _popUpAdActivator;

        private float _duration = 10f;
        private WaitForSeconds _waitForSeconds;
        private Coroutine _coroutine;

        public void Activate()
        {
            _waitForSeconds = new WaitForSeconds(_duration);
            gameObject.SetActive(true);
            
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ActivatePopUpButton());
        }

        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            _ads.ShowRewarded(() =>
            {
                AppMetrica.ReportEvent("RewardAD", "{\"" + "PopUpMoneyDisappearing" + "\":null}");
                _wallet.Add(new DollarValue(50, 00));
                gameObject.SetActive(false);
                _popUpAdActivator.StartTiming();
            });
        }

        private IEnumerator ActivatePopUpButton()
        {
            yield return _waitForSeconds;
            _popUpAdActivator.StartTiming();
            gameObject.SetActive(false);
        }
    }
}