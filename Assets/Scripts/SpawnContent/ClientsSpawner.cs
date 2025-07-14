using System.Collections.Generic;
using ClientsContent;
using Enums;
using TutorialContent;
using UnityEngine;

namespace SpawnContent
{
    public class ClientsSpawner : MonoBehaviour
    {
        [SerializeField] private Client[] _clientPrefabs;
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private Transform _spawnTutorPosition;
        [SerializeField] private int _spawnAmount;
        [SerializeField] private Tutorial _tutorial;

        private ObjectPool<Client> _clientPool;
        private List<ObjectPool<Client>> _clientPools = new List<ObjectPool<Client>>();

        private void Start()
        {
            /*_clientPool = new ObjectPool<Client>(_clientPrefabs[0], _spawnAmount, _container);
            _clientPool.EnableAutoExpand();*/

            foreach (var clientPrefab in _clientPrefabs)
            {
                var clientPool = new ObjectPool<Client>(clientPrefab, _spawnAmount, _container);
                clientPool.EnableAutoExpand();
                _clientPools.Add(clientPool);
            }
        }

        public Client SpawnRandomClient()
        {
            ObjectPool<Client> randomPool = _clientPools[Random.Range(0, _clientPools.Count)];
            Client client = randomPool.GetFirstObject();

            if (client == null)
            {
                client = Instantiate(_clientPrefabs[0], _container);
                SetPosition(client);
            }
            else
            {
                SetPosition(client);
                client.gameObject.SetActive(true);
            }

            return client;
        }

        private void SetPosition(Client client)
        {
            if (_tutorial.CurrentType == TutorialType.TakeFirstOrder)
            {
                client.transform.position = _spawnTutorPosition.position;
                client.transform.rotation = _spawnTutorPosition.rotation;
            }
            else
            {
                client.transform.position = _spawnPosition.position;
                client.transform.rotation = _spawnPosition.rotation;
            }
        }
    }
}