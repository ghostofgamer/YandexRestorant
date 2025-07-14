using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace WorkerContent
{
    public class WorkerMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private WorkerAnimation _workerAnimation;
        [SerializeField] private Worker _worker;
        [SerializeField] private float _baseSpeed;

        private Coroutine _coroutine;

        public NavMeshAgent Agent => _agent;

        public void MoveTarget(Transform target, Action onArrived)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(MoveToTarget(target, onArrived));
        }

        private IEnumerator MoveToTarget(Transform target, Action onArrived)
        {
            _agent.SetDestination(target.position);
            _workerAnimation.SetWalkAnimValue(true);

            while (_agent.pathPending || _agent.remainingDistance > 0.1f)
                yield return null;

            transform.rotation = target.rotation;
            _workerAnimation.SetWalkAnimValue(false);
            onArrived?.Invoke();
        }

        public void SetSpeed(int level)
        {
            _agent.speed = _baseSpeed * _worker.WorkerParametersConfig.GetConfig(_worker.WorkerType, level).Speed;
        }
    }
}