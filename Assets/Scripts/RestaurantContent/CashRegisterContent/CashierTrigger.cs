using ClientsContent;
using UnityEngine;

namespace RestaurantContent.CashRegisterContent
{
    public class CashierTrigger : MonoBehaviour
    {
        [SerializeField] private CashRegister _cashRegister;

        private void OnTriggerEnter(Collider other)
        {
            /*Client client = other.GetComponent<Client>();

            if (client != null && client.CanInteractWithCashier())
            {
                Debug.Log("Клиент готов платить");
                _cashRegister.SetClient(client);
            }*/
        }
    }
}