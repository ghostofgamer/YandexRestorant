using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LoadingSceneContent
{
    public class LoadingGame : MonoBehaviour
    {
        [SerializeField] private GameObject _playButton;
        [SerializeField] private GameObject _sliderLoader;
        [SerializeField] private Image _loadingBar;
        [SerializeField] private float _loadingTime = 3f;

        void Start()
        {
            StartCoroutine(LoadAsync());
        }

        IEnumerator LoadAsync()
        {
            float elapsedTime = 0f;
            float fillAmount = 0f;

            while (elapsedTime < _loadingTime)
            {
                elapsedTime += Time.deltaTime;
                fillAmount = Mathf.Clamp01(elapsedTime / _loadingTime);
                _loadingBar.fillAmount = fillAmount;
                yield return null;
            }
            
            _playButton.SetActive(true);
            _sliderLoader.gameObject.SetActive(false);
        }
    }
}