using System;
using RestaurantContent;
using RestaurantContent.TrayContent;
using UnityEngine;

namespace SpawnContent
{
    public class TraySpawner : MonoBehaviour
    {
        [SerializeField] private Tray _trayPrefabs;
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private int _spawnCount;
        
        private ObjectPool<Tray> _trayPool;

        private void Awake()
        {
            _trayPool = new ObjectPool<Tray>(_trayPrefabs, _spawnCount, _container);
            _trayPool.EnableAutoExpand();
        }

        /*private void Start()
        {
            _trayPool = new ObjectPool<Tray>(_trayPrefabs, _spawnCount, _container);
            _trayPool.EnableAutoExpand();
        }*/
        
        public Tray SpawnTray()
        {
            Tray client = _trayPool.GetFirstObject();

            if (client == null)
            {
                client = Instantiate(_trayPrefabs, _container);
            }
            else
            {
                client.transform.position = _spawnPosition.position;
                client.transform.rotation = _spawnPosition.rotation;
                client.gameObject.SetActive(true);
            }

            return client;
        }
    }
}
