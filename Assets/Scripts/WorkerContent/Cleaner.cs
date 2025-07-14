using System;
using System.Collections;
using Enums;
using RestaurantContent;
using RestaurantContent.TableContent;
using UnityEngine;
using UnityEngine.AI;
using WorkerContent.FSM;

namespace WorkerContent
{
    public class Cleaner : Worker
    {
        [SerializeField] private DirtyCounter _dirtyCounter;
        [SerializeField] private NavMeshObstacle _navMeshObstacle;

        private Coroutine _cleaningCoroutine;
        private WaitForSeconds _waitForSeconds;
        private float _baseEfficiecy = 100;

        public TableCleanliness CurrentDirtyTable { get; private set; }

        private void Start()
        {
            Activate();
        }

        public override bool GetConditionsWorkUpdate()
        {
            return CurrentDirtyTable != null;
        }

        public override bool GetConditionsRelaxUpdate()
        {
            return CurrentDirtyTable == null && IsTired;
        }

        public override void Deactivate()
        {
            CurrentDirtyTable = null;
            base.Deactivate();
        }

        public override void StartWorking()
        {
            if (CurrentWorkerStateType == WorkerStateType.Relax)
                return;

            if (CurrentDirtyTable != null)
                return;

            if (CurrentDirtyTable == null)
                FindDirtyTable();
        }

        public override void StartRelaxing(Action action)
        {
            if (WorkerMover.Agent.destination != RelaxPosition.position)
            {
                WorkerMover.MoveTarget(RelaxPosition, () =>
                {
                    if (action != null)
                        action?.Invoke();
                });
            }
        }

        public override void StartRelax()
        {
        }

        private void FindDirtyTable()
        {
            TableCleanliness dirtyTable = _dirtyCounter.GetDirtyTable();

            if (dirtyTable == null)
            {
                StartRelaxing(null);
                return;
            }

            CurrentDirtyTable = dirtyTable;
            
            WorkerMover.MoveTarget(CurrentDirtyTable.CleanerPosition,
                () => StartCoroutine(CleanTable()));
        }

        private IEnumerator CleanTable()
        {
            float newTime = StartEfficiencySecValue * (_baseEfficiecy / Efficiecy);
            _waitForSeconds = new WaitForSeconds(newTime);
            Debug.Log("убираюсь " + newTime);

            WorkerAnimation.SetCleaningAnimValue(true);
            yield return _waitForSeconds;

            if (CurrentDirtyTable != null && CurrentDirtyTable.PollutionLevel > 0)
                CurrentDirtyTable.ClearTable();

            WorkerAnimation.SetCleaningAnimValue(false);
            CurrentDirtyTable = null;

            if (CurrentState is WorkState)
                FindDirtyTable();
            else
                StartRelaxing(() => SetValueTired(true));
        }
    }
}