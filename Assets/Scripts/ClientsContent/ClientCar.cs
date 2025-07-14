using System.Collections;
using System.Collections.Generic;
using ParkingContent;
using UnityEngine;
using UnityEngine.AI;

namespace ClientsContent
{
    public class ClientCar : MonoBehaviour
    {
        [SerializeField] private List<Client> _clients = new List<Client>();
        [SerializeField] private Transform[] _position;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private ParkingSpace _parkingSpace;
        [SerializeField] private Transform _exitPosition;

        private Transform _exitCarPosition;

        private int _maxSize = 3;
        private Coroutine _coroutine;

        public Transform ExitPosition => _exitPosition;

        public void AddClient(Client client, ParkingSpace parkingSpace, Transform exitCarPosition)
        {
            if (_clients.Count >= _maxSize)
                return;

            _clients.Add(client);
            _parkingSpace = parkingSpace;
            client.gameObject.SetActive(false);
            _exitCarPosition = exitCarPosition;
        }

        public void ActivateClients()
        {
            for (int i = 0; i < _clients.Count; i++)
            {
                _clients[i].gameObject.SetActive(true);
                _clients[i].gameObject.transform.position = _position[i].position;
                _clients[i].UpdateGotoQueue();
            }

            Debug.Log("АКТИВИРУЕМ НАШИХ КЛИЕНТОВ");
        }

        public void GoToPosition(Vector3 target)
        {
            // _navMeshAgent.SetDestination(target);


            SetDestination(target, () => { ActivateClients(); });
        }

        public void SetDestination(Vector3 position, System.Action callback)
        {
            /*_navMeshAgent.enabled = true;
            _meshObstacle.enabled = true;*/

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(MoveToPosition(position, callback));
        }

        private IEnumerator MoveToPosition(Vector3 position, System.Action callback)
        {
            // _meshObstacle.enabled = false;

            /*if (!_navMeshAgent.enabled)
            {
                _navMeshAgent.enabled = true;
                transform.position = Table.ClientStandPosition.position;
                transform.rotation = Table.ClientStandPosition.rotation;
            }*/

            _navMeshAgent.SetDestination(position);

            while (_navMeshAgent.pathPending)
                yield return null;

            while (_navMeshAgent.remainingDistance > 0.1f)
                yield return null;

            transform.rotation = _parkingSpace.transform.rotation;
            // _meshObstacle.enabled = true;
            Debug.Log("Завершил ехать ");

            callback.Invoke();
        }

        public void RemoveClient(Client client)
        {
            _clients.Remove(client);

            if (_clients.Count <= 0)
            {
                SetDestination(_exitCarPosition.position, () =>
                {
                    _parkingSpace.ClearPlace();
                    gameObject.SetActive(false);
                });
            }
        }
    }
}