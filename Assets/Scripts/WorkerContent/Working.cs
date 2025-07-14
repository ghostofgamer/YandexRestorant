using Enums;
using RestaurantContent;
using RestaurantContent.CashRegisterContent;
using RestaurantContent.TableContent;
using UnityEngine;

namespace WorkerContent
{
    public class Working : MonoBehaviour
    {
        [SerializeField] private DirtyCounter _dirtyCounter;
        [SerializeField] private Workers _workers;
        [SerializeField] private CashRegister _cashRegister;

        private void OnEnable()
        {
            _dirtyCounter.DirtyTableAdded += CallCleaner;
            _cashRegister.PlayerOnSiteDisabled += CallCashier;
        }

        private void OnDisable()
        {
            _dirtyCounter.DirtyTableAdded -= CallCleaner;
            _cashRegister.PlayerOnSiteDisabled -= CallCashier;
        }

        private void CallCleaner()
        {
            TableCleanliness dirtyTable = _dirtyCounter.GetDirtyTable();
            Worker cleaner = _workers.GetWorker(WorkerType.Cleaner);

            if (cleaner != null)
            {
                if (dirtyTable != null)
                    cleaner.StartWorking();
            }
            else
            {
                Debug.Log("туту Null уборщик");
                return;
            }
        }
        
        private void CallCashier()
        {
            Worker cashier = _workers.GetWorker(WorkerType.Cashier);

            if (cashier != null)
                cashier.StartWorking();
            else
                Debug.Log("THIS Null cashier");
        }
    }
}