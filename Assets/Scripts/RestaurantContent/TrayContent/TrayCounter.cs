using System.Collections.Generic;
using SpawnContent;
using UnityEngine;
using System.Linq;
using KitchenEquipmentContent.AssemblyTables.CoffeeTableContent;
using KitchenEquipmentContent.AssemblyTables.SodaTableContent;
using KitchenEquipmentContent.FryerContent;
using OrdersContent;

namespace RestaurantContent.TrayContent
{
    public class TrayCounter : MonoBehaviour
    {
        [SerializeField] private TraySpawner _traySpawner;
        [SerializeField] private Transform[] _trayPositions;
        [SerializeField] private OrdersCounter _ordersCounter;
        [SerializeField] private BurgersCounter _burgersCounter;
        [SerializeField] private CoffeeCounter _coffeeCounter;
        [SerializeField] private SodaCounter _sodaCounter;
        [SerializeField] private DeepFryerItemCounter _deepFryerItemCounter;

        private List<Tray> _activeTrays = new List<Tray>();
        private Dictionary<Tray, int> _trayToPositionMap = new Dictionary<Tray, int>();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < _trayPositions.Length; i++)
            {
                Tray tray = _traySpawner.SpawnTray();
                tray.Init(_ordersCounter, _traySpawner.transform, _burgersCounter, _coffeeCounter, _sodaCounter,_deepFryerItemCounter);
                tray.Clear();
                _activeTrays.Add(tray);
                _trayToPositionMap[tray] = i;
                tray.gameObject.SetActive(true);
                tray.transform.position = _trayPositions[i].position;
                tray.transform.rotation = _trayPositions[i].rotation;
            }
        }

        public void UpdateTrayList(Tray takenTray)
        {
            if (takenTray != null && _trayToPositionMap.ContainsKey(takenTray))
            {
                int positionIndex = _trayToPositionMap[takenTray];

                // Remove the taken tray from the active list and dictionary
                // takenTray.DefaultReturn();
                _activeTrays.Remove(takenTray);
                _trayToPositionMap.Remove(takenTray);

                // Spawn a new tray and place it at the position of the taken tray
                Tray newTray = _traySpawner.SpawnTray();
                newTray.Init(_ordersCounter, _traySpawner.transform, _burgersCounter, _coffeeCounter, _sodaCounter,_deepFryerItemCounter);
                newTray.Clear();
                _activeTrays.Add(newTray);
                _trayToPositionMap[newTray] = positionIndex;
                newTray.gameObject.SetActive(true);
                newTray.transform.position = _trayPositions[positionIndex].position;
                newTray.transform.rotation = _trayPositions[positionIndex].rotation;
                _ordersCounter.TryActivateOrder();
            }
        }

        public int GetFreeTrayCount()
        {
            int freeTraysCount = _activeTrays.Count(tray => !tray.IsBusy);
            return freeTraysCount;
        }

        public Tray GetTray()
        {
            Tray freeTray = _activeTrays.FirstOrDefault(tray => !tray.IsBusy);
            return freeTray;
        }

        public Tray GetTrayByTableIndex(Order order)
        {
            Tray needTray = _activeTrays.FirstOrDefault(tray => tray.Order == order);
            return needTray;
        }
    }
}