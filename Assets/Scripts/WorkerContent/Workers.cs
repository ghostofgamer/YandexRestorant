using Enums;
using LoadingSceneContent;
using SaveContent;
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
        [SerializeField]private LoadingGame _loadingGame;

        private void OnEnable()
        {
            foreach (var workerUIProduct in _workerUIProducts)
            {
                workerUIProduct.WorkerBuyed += ActivateWorker;
                workerUIProduct.WorkerFired += DeactivateWorker;
            }
            
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            foreach (var workerUIProduct in _workerUIProducts)
            {
                workerUIProduct.WorkerBuyed -= ActivateWorker;
                workerUIProduct.WorkerFired -= DeactivateWorker;
            }
            
            _loadingGame.MirraSDKInitialization -= Init;
        }

        /*private void Start()
        {
            Debug.Log("работник " + PlayerPrefs.GetInt(Worker + WorkerType.Cleaner, 0));

            foreach (var worker in _workers)
                worker.gameObject.SetActive(PlayerPrefs.GetInt(Worker + worker.WorkerType, 0) > 0);
        }*/

        private void Init()
        {
            Debug.Log("работник " + StorageHelper.GetInt(Worker + WorkerType.Cleaner, 0));

            foreach (var worker in _workers)
            {
                bool isActive = StorageHelper.GetInt(Worker + worker.WorkerType, 0) > 0;
                worker.gameObject.SetActive(isActive);
            }
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