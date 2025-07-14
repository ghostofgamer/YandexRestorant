using System.Collections;
using TMPro;
using UnityEngine;

namespace AttentionHintContent
{
    public class AttentionHintViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _hintText;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);
        private float _duration = 0.65f;
        private float _currentTime = 0f;
        private float _startAlpha = 1f;
        private float _endAlpha = 0f;

        public void ShowAttentionHint(string message)
        {
            gameObject.SetActive(true);
            _hintText.text = message;
            _canvasGroup.alpha = _startAlpha; 

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(FadeOutHint(message));
        }

        private IEnumerator FadeOutHint(string message)
        {
            yield return _waitForSeconds;

            _currentTime = 0f;

            while (_currentTime < _duration)
            {
                _currentTime += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(_startAlpha, _endAlpha, _currentTime / _duration);
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}