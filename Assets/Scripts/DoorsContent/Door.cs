using System;
using UnityEngine;

namespace DoorsContent
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private DoorAnimation _doorAnimation;
        
        public event Action OpenedDoor;
        public event Action ClosedDoor;

        public void Open()
        {
            _doorAnimation.OpeningAnim();
        }

        public void Close()
        {
            _doorAnimation.ClosingAnim();
        }
        
        public void Opened()
        {
            Debug.Log("Opened");
            OpenedDoor?.Invoke();
        }

        public void Closed()
        {
            Debug.Log("Closed");
            ClosedDoor?.Invoke();
        }
    }
}