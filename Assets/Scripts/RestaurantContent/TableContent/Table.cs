using UnityEngine;

namespace RestaurantContent.TableContent
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private int _index;
        [SerializeField] private Transform _clientSitPosition;
        [SerializeField] private Transform _clientStandPosition;
        [SerializeField] private Transform _trayPosition;
        [SerializeField] private TableCleanliness _tableCleanliness;

        public Transform ClientSitPosition => _clientSitPosition;

        public Transform ClientStandPosition => _clientStandPosition;

        public Transform TrayPosition => _trayPosition;

        public bool IsBusy { get; private set; }

        public int Index => _index;

        public void SetBusyValue(bool value)
        {
            IsBusy = value;
        }

        public void DirtyTable()
        {
            _tableCleanliness.PolluteTable();
        }
    }
}