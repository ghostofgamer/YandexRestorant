using LoadingSceneContent;
using MirraGames.SDK;
using SaveContent;
using UnityEngine;
using UnityEngine.UI;

namespace IAP
{
    public class UIInfo : MonoBehaviour
    {
        [SerializeField] private GameObject _removeAdsButton;
        [SerializeField] private LoadingGame _loadingGame;

        private void OnEnable()
        {
            _loadingGame.MirraSDKInitialization += Init;
            
            if (MirraSDK.IsInitialized)
                UpdateRemoveAdsButton();
        }

        private void OnDisable()
        {
            _loadingGame.MirraSDKInitialization -= Init;
        }

        /*private void Start()
        {
            UpdateRemoveAdsButton();
        }*/

        private void Init()
        {
            UpdateRemoveAdsButton();
        }

        public void UpdateRemoveAdsButton()
        {
            // bool removeAds = PlayerPrefs.GetInt("removeADS") == 1;
            bool removeAds = StorageHelper.GetInt("removeADS") == 1;
            _removeAdsButton.GetComponent<Button>().interactable = !removeAds;
        }
    }
}