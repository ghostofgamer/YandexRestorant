using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClientsContent
{
    public class ClientsCounter : MonoBehaviour
    {
        [SerializeField] private List<Client> _clients = new List<Client>();

        public List<Client> Clients => _clients;

        public event Action ClientsListEmpted;

        public void AddClient(Client client)
        {
            _clients.Add(client);
        }

        public void RemoveClient(Client client)
        {
            _clients.Remove(client);

            if (_clients.Count <= 0)
                ClientsListEmpted?.Invoke();
        }
    }
}