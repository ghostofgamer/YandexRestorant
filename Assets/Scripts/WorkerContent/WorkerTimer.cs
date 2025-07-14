using System;
using Enums;
using UnityEngine;

namespace WorkerContent
{
    [RequireComponent(typeof(Worker), typeof(WorkerTimerViewer))]
    public class WorkerTimer : MonoBehaviour
    {
        [SerializeField] private Worker _worker;
        [SerializeField] private WorkerTimerViewer _workerTimerViewer;

        private float _delayRelax;
        private float _delayWork;

        public event Action<WorkerStateType,float> ValueChanged;
        
        public float StateTimer { get; private set; }

        public void SetTimeWork()
        {
            _delayWork = _worker.WorkerParametersConfig.GetConfig(_worker.WorkerType,_worker.Level).DelayWork;
            StateTimer = _delayWork;
            ValueChanged?.Invoke(WorkerStateType.Work,StateTimer);
        }

        public void SetStateRelax()
        {
            _delayRelax = _worker.WorkerParametersConfig.GetConfig(_worker.WorkerType,_worker.Level).DelayRelax;
            StateTimer = _delayRelax;
            ValueChanged?.Invoke(WorkerStateType.Relax,StateTimer);
        }

        public void WakeUpWorker()
        {
            StateTimer = 0;
        }

        public void Init(WorkerStateType workerStateType)
        {
            switch (workerStateType)
            {
                case WorkerStateType.Work:
                    SetTimeWork();
                    _workerTimerViewer.UpdateTimerView(StateTimer, WorkerStateType.Work, _delayWork);
                    break;
                case WorkerStateType.Relax:
                    SetStateRelax();
                    _workerTimerViewer.UpdateTimerView(StateTimer, WorkerStateType.Relax, _delayRelax);
                    break;
            }
        }

        public void UpdateViewInfo(WorkerStateType workerStateType)
        {
            switch (workerStateType)
            {
                case WorkerStateType.Work:
                    StateTimer -= Time.deltaTime;
                    _workerTimerViewer.UpdateTimerView(StateTimer, WorkerStateType.Work, _delayWork);
                    ValueChanged?.Invoke(WorkerStateType.Work,StateTimer);
                    break;
                case WorkerStateType.Relax:
                    StateTimer -= Time.deltaTime;
                    _workerTimerViewer.UpdateTimerView(StateTimer, WorkerStateType.Relax, _delayRelax);
                    ValueChanged?.Invoke(WorkerStateType.Relax,StateTimer);
                    break;
            }
        }
    }
}