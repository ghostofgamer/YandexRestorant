using System;
using Enums;
using LoadingSceneContent;
using SaveContent;
using TutorialContent;
using UI.Screens;
using UnityEngine;

namespace NewsUpdateContent
{
    public class NewsActivator : MonoBehaviour
    {
        [SerializeField] private NewsScreen _newsScreen;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private LoadingGame _loadingGame;

        private void OnEnable()
        {
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            _loadingGame.MirraSDKInitialization -= Init;
        }

        /*private void Start()
        {
            // int value = PlayerPrefs.GetInt("Update1.0.0", 0);
            int value = StorageHelper.GetInt("Update1.0.0", 0);
            
            if (value == 0 && (int)_tutorial.CurrentType >= (int)TutorialType.TutorCompleted)
                OpenScreen();
        }*/

        private void Init()
        {
            int value = StorageHelper.GetInt("Update1.0.0", 0);
            
            if (value == 0 && (int)_tutorial.CurrentType >= (int)TutorialType.TutorCompleted)
                OpenScreen();
        }

        private void OpenScreen()
        {
            _newsScreen.OpenScreen();
            // PlayerPrefs.SetInt("Update1.0.0", 1);
            StorageHelper.SetInt("Update1.0.0", 1);
        }
    }
}