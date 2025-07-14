using Enums;
using TutorialContent;
using UI.Screens;
using UnityEngine;

namespace NewsUpdateContent
{
    public class NewsActivator : MonoBehaviour
    {
        [SerializeField] private NewsScreen _newsScreen;
        [SerializeField] private Tutorial _tutorial;

        private void Start()
        {
            int value = PlayerPrefs.GetInt("Update2.0.9", 0);

            if (value == 0 && (int)_tutorial.CurrentType >= (int)TutorialType.TutorCompleted)
                OpenScreen();
        }

        private void OpenScreen()
        {
            _newsScreen.OpenScreen();
            PlayerPrefs.SetInt("Update2.0.9", 1);
        }
    }
}