using Enums;
using UnityEngine;

namespace WorkerContent
{
    public class WorkerButtonChanger : MonoBehaviour
    {
        [SerializeField] private GameObject _icon;
        [SerializeField] private Animator _animator;
        [SerializeField] private Worker[] _workers;

        private int _relaxWorkersValue;

        private void OnEnable()
        {
            foreach (var worker in _workers)
                worker.StateChanged += StateChange;
        }

        private void OnDisable()
        {
            foreach (var worker in _workers)
                worker.StateChanged -= StateChange;
        }

        private void Start()
        {
            StateChange();
        }

        private void StateChange()
        {
            _relaxWorkersValue = 0;

            foreach (var worker in _workers)
            {
                if (worker.CurrentWorkerStateType == WorkerStateType.Relax && worker.gameObject.activeSelf)
                    _relaxWorkersValue++;
            }

            ChangeValue(_relaxWorkersValue > 0);
        }

        private void ChangeValue(bool value)
        {
            _icon.SetActive(value);
            _animator.enabled = value;
        }
    }
}