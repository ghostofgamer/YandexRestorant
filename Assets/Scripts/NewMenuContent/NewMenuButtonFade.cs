using System.Collections;
using UnityEngine;

namespace NewMenuContent
{
    public class NewMenuButtonFade : MonoBehaviour
    {
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(60f);

        private void OnEnable()
        {
            StartFade();
        }

        public void Disable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            gameObject.SetActive(false);
        }

        private void StartFade()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            yield return _waitForSeconds;
            gameObject.SetActive(false);
        }
    }
}