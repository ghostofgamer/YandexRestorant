using System.Collections.Generic;
using UnityEngine;

namespace ClientsContent
{
    public class QueueCashRegister : MonoBehaviour
    {
        [SerializeField] public Transform[] _queuePositions;
        [SerializeField] private List<Client> _clientListForInspector = new List<Client>();
        
        private Queue<Client> clientQueue = new Queue<Client>();
        private Client currentClient;
        private int _maxQueueSize = 5;

        public Queue<Client> ClientQueue => clientQueue;

        public void AddClientToQueue(Client client)
        {
            clientQueue.Enqueue(client);
            UpdateQueuePositions();
            UpdateClientList();
        }

        public void AddClientQueue(Client client)
        {
            clientQueue.Enqueue(client);
            UpdateClientList();
        }

        [ContextMenu("UpdateQueuePositions")]
        public void UpdateQueuePositions()
        {
            int index = 0;

            foreach (var client in clientQueue)
            {
                if (index < _queuePositions.Length)
                {
                    if (client.gameObject.activeSelf)
                    {
                        client.GoToQueuePosition(_queuePositions[index].position, index);
                    }

                    index++;
                }
            }
        }

        [ContextMenu("ClientFinishedOrder")]
        public void ClientFinishedOrder()
        {
            if (clientQueue.Count > 0)
            {
                Client client = clientQueue.Dequeue();
                UpdateQueuePositions();
            }
        }

        public bool IsQueueFull()
        {
            return clientQueue.Count >= _maxQueueSize;
        }

        public int GetFreeQueuePositions()
        {
            return _maxQueueSize - clientQueue.Count;
        }
        
        private void UpdateClientList()
        {
            _clientListForInspector.Clear();
            
            foreach (var client in clientQueue)
            {
                _clientListForInspector.Add(client);
            }
        }
    }
}