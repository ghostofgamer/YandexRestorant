using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace IAP
{
    public class UIInfo : MonoBehaviour
    {
        [SerializeField] private GameObject _removeAdsButton;

        private void OnEnable()
        {
            UpdateRemoveAdsButton();
        }

        private void Start()
        {
            UpdateRemoveAdsButton();
        }

        public void UpdateRemoveAdsButton()
        {
            bool removeAds = PlayerPrefs.GetInt("removeADS") == 1;
            _removeAdsButton.GetComponent<Button>().interactable = !removeAds;
        }
    }
}