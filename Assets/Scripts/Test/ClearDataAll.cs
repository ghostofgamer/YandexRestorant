using SaveContent;
using TutorialContent;
using UI.Buttons;
using UI.Screens;
using UnityEngine;

namespace Test
{
    public class ClearDataAll : AbstractButton
    {
        [SerializeField] private BoxSaver _boxSaver;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField]private FirstLanguageScreen _firstLanguageScreen;


        public override void OnClick()
        {
            _tutorial.ClearSaveData();
            _boxSaver.ClearSavedData();
            _firstLanguageScreen.ClearSaveData();
        }
    }
}