using DoorsContent;
using InteractableContent;
using PlayerContent;
using RestaurantContent;
using UnityEngine;

namespace StorageContent
{
    public class Storage : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private OcclusionPortal _occlusionPortal;
        [SerializeField] private Door _door;
        [SerializeField] private ZoneWall _zoneWall;
        [SerializeField] private GameObject _lockImage;

        private bool _isOpen = false;
        private bool _isOpens;
        private bool _isCloses;
        public bool IsOpened { get; private set; } = false;

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
            _door.OpenedDoor += OpenedCompleted;
            _door.ClosedDoor += CloseCompleted;
            _zoneWall.ActivityDoorChanged += SetBuyedValue;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
            _door.OpenedDoor -= OpenedCompleted;
            _door.ClosedDoor -= CloseCompleted;
            _zoneWall.ActivityDoorChanged -= SetBuyedValue;
        }

        private void SetBuyedValue(bool value)
        {
            IsOpened = value;
            _lockImage.SetActive(!value);
        }

        private void StorageOpen()
        {
            Debug.Log("Open");
            _occlusionPortal.open = true;
        }

        private void StorageClose()
        {
            Debug.Log("Close");
            _occlusionPortal.open = false;
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            if (!IsOpened)
            {
                Debug.Log("Не купил еще");
                return;
            }

            if (_isOpens || _isCloses)
                return;

            if (_isOpen)
            {
                _isCloses = true;
                _door.Close();
                _isOpen = false;
            }
            else
            {
                _isOpens = true;
                _door.Open();
                _isOpen = true;
                StorageOpen();
            }
        }

        private void OpenedCompleted()
        {
            _isOpens = false;
        }

        private void CloseCompleted()
        {
            _isCloses = false;
            StorageClose();
        }
    }
}