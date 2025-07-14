using ClientsContent;
using UnityEngine;

namespace ParkingContent
{
    public class ParkingSpace : MonoBehaviour
    {
        private ClientCar _clientCar;

        public bool IsBusy { get; private set; }

        public void BusyPlace(ClientCar clientCar)
        {
            IsBusy = true;
            _clientCar = clientCar;
        }

        public void ClearPlace()
        {
            _clientCar = null;
            IsBusy = false;
        }
    }
}