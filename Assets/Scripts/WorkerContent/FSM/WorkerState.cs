using System;

namespace WorkerContent.FSM
{
    public abstract class WorkerState
    {
        public abstract void Enter(Worker worker,Action action);
        public abstract void Update(Worker worker);
        public abstract void Exit(Worker worker);
    }
}