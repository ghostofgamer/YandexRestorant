using UnityEngine;

namespace DeliveryContent
{
    public class SkipCounter : MonoBehaviour
    {
        [SerializeField] private GameObject _skipFreeButton;
        [SerializeField] private GameObject _skipAdButton;

        private bool _isFirstSkip = true;

        private void OnEnable()
        {
            Debug.Log("OnEnableDelivery");
            Show();
        }

        private void Start()
        {
            int value = PlayerPrefs.GetInt("SkipFirstActivate", 0);
            _isFirstSkip = value <= 0;
            Show();
        }

        public void SkipFirstActivate()
        {
            if (_isFirstSkip)
            {
                _isFirstSkip = false;
                PlayerPrefs.SetInt("SkipFirstActivate", 1);
            }
        }

        private void Show()
        {
            _skipFreeButton.SetActive(_isFirstSkip);
            _skipAdButton.SetActive(!_isFirstSkip);
        }
    }
}