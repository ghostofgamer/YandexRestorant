using FortuneContent;
using UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace TaskContent
{
    public class FortuneTask : GameTaskAbstract
    {
        [SerializeField] private FortuneTaskScreen _fortuneTaskScreen;
        [SerializeField] private GameObject _touchFortune;
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Fortune _fortune;

        private void OnEnable()
        {
            _fortune.FreeSpinUsed += CompletedTask;
        }

        private void OnDisable()
        {
            _fortune.FreeSpinUsed -= CompletedTask;
        }

        public override void ActivateTask()
        {
            foreach (var button in _buttons)
                button.interactable = false;
            
            _fortuneTaskScreen.OpenScreen();
        }

        public override void CompletedTask()
        {
            foreach (var button in _buttons)
                button.interactable = true;
            
            _touchFortune.SetActive(false);
            _fortuneTaskScreen.CloseScreen();
        }
    }
}