using TMPro;
using UnityEngine;
using WorkerContent;
using WorkerContent.WorkerWakeUpContent;

namespace UI.Screens
{
    public class WorkersScreen : AbstractScreen
    {
        [SerializeField] private WorkerAwakening[] _workerAwakenings;
        [SerializeField] private TMP_Text _notActiveWorkerText;

        private int _activeWorkerValue;

        public override void OpenScreen()
        {
            _activeWorkerValue = 0;
            base.OpenScreen();

            foreach (var workerAwakening in _workerAwakenings)
            {
                workerAwakening.Init();

                if (workerAwakening.gameObject.activeSelf)
                    _activeWorkerValue++;
            }


            _notActiveWorkerText.gameObject.SetActive(_activeWorkerValue <= 0);
        }

        public override void CloseScreen()
        {
            base.CloseScreen();

            foreach (var workerAwakening in _workerAwakenings)
                workerAwakening.Unsubscribe();
        }

        public void DeactivateSubscribe(Worker worker)
        {
            foreach (var workerAwakening in _workerAwakenings)
            {
                if (workerAwakening.Worker == worker)
                    workerAwakening.Unsubscribe();
            }
        }
    }
}