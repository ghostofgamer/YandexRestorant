using ADSContent;
using AttentionHintContent;
using EnergyContent;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace WorkerContent.WorkerWakeUpContent
{
    public class WorkerAwakening : MonoBehaviour
    {
        [SerializeField] private Worker _worker;
        [SerializeField] private int _energyPrice;
        [SerializeField] private ADS _ads;
        [SerializeField] private Energy _energy;
        [SerializeField] private Button[] _buttons;
        [SerializeField] private ButtonColorChanger[] _buttonsColorChangers;
        [SerializeField] private WorkerAwakeningViewer _workerAwakeningViewer;
        [SerializeField] private WorkerTimer _workerTimer;

        private bool _subscribe;

        public Worker Worker => _worker;

        public void Init()
        {
            gameObject.SetActive(_worker.gameObject.activeSelf);

            if (!_worker.gameObject.activeSelf)
                return;

            ChangeButtonsValue(_worker.CurrentWorkerStateType);
            ChangeValue(_worker.CurrentWorkerStateType, _workerTimer.StateTimer);

            if (!_subscribe)
            {
                _workerTimer.ValueChanged += ChangeValue;
                _subscribe = true;
            }
        }

        public void Wake(bool adsValue)
        {
            if (adsValue)
            {
                if (_worker.CurrentWorkerStateType == WorkerStateType.Relax)
                    _ads.ShowRewarded(() =>
                    {
                        // AppMetrica.ReportEvent("RewardAD", "{\"" + "WakeUpWorkerADS" + "\":null}");
                        _worker.WakeUp();
                    });
                else
                    Debug.Log("Он и так в состоянии работы");
            }
            else
            {
                if (_energy.EnergyValue < _energyPrice)
                {
                    Debug.Log("недостаточно енергии");
                    AttentionHintActivator.Instance.ShowHint("недостаточно енергии");
                    return;
                }

                if (_worker.CurrentWorkerStateType == WorkerStateType.Work)
                {
                    Debug.Log("Он и так в состоянии работы");
                    return;
                }

                // AppMetrica.ReportEvent("Energy", "{\"" + "WakeUpWorkerEnergy" + "\":null}");
                _energy.DecreaseEnergy(_energyPrice);
                _worker.WakeUp();
            }
        }

        public void Unsubscribe()
        {
            if (_subscribe)
            {
                _subscribe = false;
                _workerTimer.ValueChanged -= ChangeValue;
            }
        }

        private void ChangeValue(WorkerStateType type, float value)
        {
            _workerAwakeningViewer.ShowInfo(type, value);
            ChangeButtonsValue(type);
        }

        private void ChangeButtonsValue(WorkerStateType type)
        {
            foreach (var button in _buttons)
                button.interactable = type == WorkerStateType.Relax;

            foreach (var button in _buttonsColorChangers)
            {
                if (type == WorkerStateType.Relax)
                    button.SetDefaultColor();
                else
                    button.SetTargetColor();
            }
        }
    }
}