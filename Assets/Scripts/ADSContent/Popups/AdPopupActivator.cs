using System.Collections;
using Enums;
using PlayerContent.LevelContent;
using TutorialContent;
using UI.Screens.AdsScreens;
using UnityEngine;

namespace ADSContent.Popups
{
    public class AdPopupActivator : MonoBehaviour
    {
        [SerializeField] private StarterPackScreen _starterPackScreen;
        [SerializeField] private StoragePackScreen _storagePackScreen;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private GameObject _starterPackButton;
        [SerializeField] private GameObject _storagePackButton;
        [SerializeField] private PlayerLevel _playerLevel;

        private Coroutine _starterPackCoroutine;
        private WaitForSeconds _waitForSecondsStarterPack = new WaitForSeconds(6f);

        private void OnEnable()
        {
            _tutorial.TutorCompleted += ShowStarterPack;
            _playerLevel.LevelChanged += ChangeValue;
        }

        private void OnDisable()
        {
            _tutorial.TutorCompleted -= ShowStarterPack;
            _playerLevel.LevelChanged -= ChangeValue;
        }

        private void Start()
        {
            if ((int)_tutorial.CurrentType >= (int)TutorialType.TutorCompleted)
            {
                if (_playerLevel.CurrentLevel < 4)
                    ShowStarterPack();
                if (_playerLevel.CurrentLevel >= 4)
                    ShowStoragePack();
            }
        }

        private void ShowStarterPack()
        {
            int value = PlayerPrefs.GetInt("StarterPack", 0);

            if (value > 0)
                return;

            if (_starterPackCoroutine != null)
                StopCoroutine(_starterPackCoroutine);

            _starterPackCoroutine = StartCoroutine(StarterPack());
        }

        private void ShowStoragePack()
        {
            int value = PlayerPrefs.GetInt("StoragePack", 0);
            Debug.Log("value ShowStoragePack" + value);
            if (value > 0)
                return;

            _storagePackScreen.OpenScreen();
            _storagePackButton.SetActive(true);
        }

        private IEnumerator StarterPack()
        {
            yield return _waitForSecondsStarterPack;
            _starterPackScreen.OpenScreen();
            _starterPackButton.SetActive(true);
        }

        private void ChangeValue(int level)
        {
            if ((int)_tutorial.CurrentType < (int)TutorialType.TutorCompleted)
                return;

            _starterPackButton.SetActive(level < 4 && PlayerPrefs.GetInt("StarterPack", 0) <= 0);
            _storagePackButton.SetActive(level >= 4 && PlayerPrefs.GetInt("StoragePack", 0) <= 0);
        }
    }
}