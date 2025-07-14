using System.Collections;
using UnityEngine;

namespace ADSContent.Popups
{
    public class PopUpAdActivator : MonoBehaviour
    {
        [SerializeField] private AddMoneyAdButton _popUpButton;
        [SerializeField] private float _duration;

        private WaitForSeconds _waitForSeconds;
        private Coroutine _coroutine;

        private void Start()
        {
            _waitForSeconds = new WaitForSeconds(_duration);
            StartTiming();
        }

        public void StartTiming()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ActivatePopUpButton());
        }

        private IEnumerator ActivatePopUpButton()
        {
            yield return _waitForSeconds;
            _popUpButton.Activate();
        }
    }
}