using System;
using Enums;
using Io.AppMetrica;
using SoContent;
using UnityEngine;
using WorkerContent.FSM;

namespace WorkerContent
{
    public abstract class Worker : MonoBehaviour
    {
        [SerializeField] private WorkerMover _workerMover;
        [SerializeField] private WorkerType _workerType;
        [SerializeField] protected Transform RelaxPosition;
        [SerializeField] private WorkerTimer _workerTimer;
        [SerializeField] private WorkerAnimation _workerAnimation;
        [SerializeField] private WorkerParametersConfig _workerParametersConfig;

        protected WorkerState CurrentState;
        protected float Efficiecy;

        private int _maxLevel = 6;

        public event Action StateChanged;

        public WorkerParametersConfig WorkerParametersConfig => _workerParametersConfig;

        public int Level { get; private set; } = 1;
        public float StartEfficiencySecValue { get; private set; }

        public WorkerTimer WorkerTimer => _workerTimer;

        public WorkerType WorkerType => _workerType;

        public WorkerMover WorkerMover => _workerMover;

        public WorkerAnimation WorkerAnimation => _workerAnimation;

        public WorkerStateType CurrentWorkerStateType { get; private set; }

        public bool IsTired { get; private set; } = false;

        private void Start()
        {
            Level = PlayerPrefs.GetInt(_workerType + "LevelWorker", 1);
        }

        private void Update()
        {
            CurrentState?.Update(this);
        }

        public abstract bool GetConditionsWorkUpdate();

        public abstract bool GetConditionsRelaxUpdate();

        public virtual void Deactivate() => gameObject.SetActive(false);

        public virtual void Activate()
        {
            Level = PlayerPrefs.GetInt(_workerType + "LevelWorker", 1);
            _workerMover.SetSpeed(Level);
            _workerTimer.SetTimeWork();
            transform.position = RelaxPosition.position;
            gameObject.SetActive(true);
            IsTired = false;
            SetWorkerStateType(WorkerStateType.Work);
            SetState(new WorkState());
            Efficiecy = _workerParametersConfig.GetConfig(_workerType, Level).Efficiency;
            StartEfficiencySecValue = _workerParametersConfig.GetConfig(_workerType, Level).StartSecondsEfficiency;
        }

        public virtual void SetState(WorkerState newState)
        {
            CurrentState?.Exit(this);
            CurrentState = newState;
            CurrentState.Enter(this, null);
        }

        public void SetValueTired(bool value)
        {
            IsTired = value;
        }

        public void SetWorkerStateType(WorkerStateType workerStateType)
        {
            CurrentWorkerStateType = workerStateType;
            StateChanged?.Invoke();
        }

        public virtual void StartWorking()
        {
        }

        public virtual void StartRelaxing(Action action)
        {
        }

        public abstract void StartRelax();

        public void NextLevel()
        {
            if (Level >= _maxLevel)
                return;

            Level++;
            AppMetrica.ReportEvent(_workerType.ToString() + " Levels", "{\"" + Level.ToString() + "\":null}");
            PlayerPrefs.SetInt(_workerType + "LevelWorker", Level);
            _workerMover.SetSpeed(Level);
            Efficiecy = _workerParametersConfig.GetConfig(_workerType, Level).Efficiency;

            if (CurrentWorkerStateType == WorkerStateType.Work)
                _workerTimer.SetTimeWork();
            if (CurrentWorkerStateType == WorkerStateType.Relax)
                _workerTimer.SetStateRelax();

            Debug.Log("CurrentWorkerStateType " + CurrentWorkerStateType);
            _workerTimer.UpdateViewInfo(CurrentWorkerStateType);
        }

        public void WakeUp()
        {
            Debug.Log("Разбудить");

            if (CurrentWorkerStateType == WorkerStateType.Relax)
            {
                SetWorkerStateType(WorkerStateType.Work);
                SetState(new WorkState());
                // _workerTimer.SetTimeWork();
                Debug.Log("DCNFFQ");
                // _workerTimer.WakeUpWorker(); 
            }
        }
    }
}