using Enums;
using SoContent;
using UI.Screens;
using UI.Screens.ShopContent.WorkersContent;
using UnityEngine;
using WalletContent;

namespace WorkerContent
{
    public class Workers : MonoBehaviour
    {
        public const string Worker = "Worker";

        [SerializeField] private Worker[] _workers;
        [SerializeField] private WorkerUIProduct[] _workerUIProducts;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private WorkersConfig _workersConfig;
        [SerializeField] private WorkersScreen _workersScreen;

        private void OnEnable()
        {
            foreach (var workerUIProduct in _workerUIProducts)
            {
                workerUIProduct.WorkerBuyed += ActivateWorker;
                workerUIProduct.WorkerFired += DeactivateWorker;
            }
        }

        private void OnDisable()
        {
            foreach (var workerUIProduct in _workerUIProducts)
            {
                workerUIProduct.WorkerBuyed -= ActivateWorker;
                workerUIProduct.WorkerFired -= DeactivateWorker;
            }
        }

        private void Start()
        {
            Debug.Log("работник " + PlayerPrefs.GetInt(Worker + WorkerType.Cleaner, 0));

            foreach (var worker in _workers)
                worker.gameObject.SetActive(PlayerPrefs.GetInt(Worker + worker.WorkerType, 0) > 0);
        }

        public Worker GetWorker(WorkerType type)
        {
            var worker = System.Array.Find(_workers, w => w.WorkerType == type);

            if (worker != null && worker.gameObject.activeSelf)
                return worker;
            else
                return null;
        }

        private void ActivateWorker(WorkerType type)
        {
            var worker = System.Array.Find(_workers, w => w.WorkerType == type);

            if (worker != null)
                worker.Activate();
        }

        private void DeactivateWorker(WorkerType type)
        {
            var worker = System.Array.Find(_workers, w => w.WorkerType == type);

            if (worker != null)
            {
                _workersScreen.DeactivateSubscribe(worker);
                worker.Deactivate();
            }
        }

        public void PaySalary()
        {
            foreach (var worker in _workers)
            {
                if (worker.gameObject.activeSelf)
                    _wallet.Subtract(_workersConfig.GetWorkerConfig(worker.WorkerType).Salary);
            }
        }
    }
}