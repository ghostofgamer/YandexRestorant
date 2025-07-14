using System;
using Enums;
using UnityEngine;

namespace WorkerContent.FSM
{
    public class WorkState : WorkerState
    {
        public override void Enter(Worker worker, Action action)
        {
            Debug.Log("WorkEnter");
            worker.WorkerTimer.Init(WorkerStateType.Work);
            worker.StartWorking();
            
            if (action != null)
                action?.Invoke();
        }

        public override void Update(Worker worker)
        {
            if (worker.GetConditionsWorkUpdate() && worker.CurrentWorkerStateType == WorkerStateType.Work)
            {
                worker.WorkerTimer.UpdateViewInfo(WorkerStateType.Work);

                if (worker.WorkerTimer.StateTimer <= 0)
                {
                    worker.SetWorkerStateType(WorkerStateType.Relax);
                    worker.SetState(new RelaxState());
                }
            }
        }

        public override void Exit(Worker worker)
        {
        }
    }
}