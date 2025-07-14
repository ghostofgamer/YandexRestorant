using System;
using System.Collections;
using Enums;
using RestaurantContent.CashRegisterContent;
using UnityEngine;
using UnityEngine.AI;
using WorkerContent.FSM;

namespace WorkerContent
{
    public class Cashier : Worker
    {
        [SerializeField] private NavMeshObstacle _navMeshObstacle;
        [SerializeField] private CashRegister _cashRegister;

        private Coroutine _cashierCoroutine;
        private WaitForSeconds _waitForSeconds;
        private float _baseValueClean = 5f;
        private float _baseEfficiecy = 100;
        private bool _tookPosition = false;
        private bool _isCalculate = false;
        
        private void Start()
        {
            Activate();
        }

        public override bool GetConditionsWorkUpdate()
        {
            return _cashRegister.CashierOnSite && _tookPosition;
        }

        public override bool GetConditionsRelaxUpdate()
        {
            return IsTired;
        }

        public override void Deactivate()
        {
            SetValueTookPosition(false);
            base.Deactivate();
        }

        public override void StartWorking()
        {
            if (CurrentWorkerStateType == WorkerStateType.Relax)
                return;

            if (_cashRegister.PlayerOnSite)
                return;

            _cashRegister.SetCashierValue(true);

            WorkerMover.MoveTarget(_cashRegister.CashierPosition,
                () =>
                {
                    SetValueTookPosition(true);
                    _cashRegister.SetCashier(this);
                    FindClient();
                });
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
            if (_isCalculate)
                return;

            _cashRegister.SetCashierValue(false);
            _cashRegister.CleanCashier();
            SetValueTookPosition(false);
            StartRelaxing(() => SetValueTired(true));
        }

        public void FindClient()
        {
            if (!_tookPosition)
                return;

            if (_cashRegister.CurrentClient == null)
                return;

            if (_cashierCoroutine != null)
                StopCoroutine(_cashierCoroutine);

            _cashierCoroutine = StartCoroutine(CalculateClient());
        }

        private IEnumerator CalculateClient()
        {
            float newTime = StartEfficiencySecValue * (_baseEfficiecy / Efficiecy);
            _waitForSeconds = new WaitForSeconds(newTime);

            _isCalculate = true;
            WorkerAnimation.SetCalculateAnimValue(true);
            yield return _waitForSeconds;
            WorkerAnimation.SetCalculateAnimValue(false);
            _cashRegister.AcceptCashierOrder();
            _isCalculate = false;

            if (CurrentState is WorkState)
                FindClient();
            else
                StartRelax();
        }

        private void SetValueTookPosition(bool value)
        {
            _tookPosition = value;
        }
    }
}