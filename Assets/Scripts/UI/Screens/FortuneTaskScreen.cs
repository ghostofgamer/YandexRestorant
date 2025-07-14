using System;
using System.Collections;
using UnityEngine;

namespace UI.Screens
{
    public class FortuneTaskScreen : AbstractScreen
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject[] _scaledObjects;
        [SerializeField] private GameObject _touchFortune;
        
        public override void OpenScreen()
        {
            base.OpenScreen();
            StartCoroutine(FadeIn());
        }
        
        private IEnumerator FadeIn()
        {
            _canvasGroup.alpha = 0f;
            float fadeDuration = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _canvasGroup.alpha = 1f;
            _touchFortune.SetActive(true);
            
            foreach (var scaledObject in _scaledObjects)
                scaledObject.SetActive(true);
        }
    }
}