using System;
using Enums;
using UnityEngine;

namespace WorkerContent.FSM
{
    public class RelaxState : WorkerState
    {
        
        public override void Enter(Worker worker, Action action)
        {
            worker.WorkerTimer.Init(WorkerStateType.Relax);
            worker.StartRelax();
            
            if (action != null)
                action?.Invoke();
        }

        public override void Update(Worker worker)
        {
            if (worker.GetConditionsRelaxUpdate() && worker.CurrentWorkerStateType == WorkerStateType.Relax)
            {
                worker.WorkerTimer.UpdateViewInfo(WorkerStateType.Relax);

                if (worker.WorkerTimer.StateTimer <= 0)
                {
                    worker.SetValueTired(false);
                    worker.SetWorkerStateType(WorkerStateType.Work);
                    worker.SetState(new WorkState());
                }
            }
        }

        public override void Exit(Worker worker)
        {
            Debug.Log("RelaxExit");
        }
    }
}