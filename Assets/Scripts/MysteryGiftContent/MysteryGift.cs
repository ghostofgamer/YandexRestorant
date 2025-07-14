using System;
using CameraContent;
using InteractableContent;
using PlayerContent;
using UnityEngine;

namespace MysteryGiftContent
{
    public class MysteryGift : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Transform _cameraPosition;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        
        public event Action BoxActivation;
        public event Action BoxDeactivation;
        
        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }

        public void DeactivateBox()
        {
            _cameraPositionChanger.ReturnDefaultPosition();
            BoxDeactivation?.Invoke();
            gameObject.SetActive(false); 
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            _cameraPositionChanger.ChangePosition(_cameraPosition);
            Debug.Log("Активирую мистический бокс");
            BoxActivation?.Invoke();
        }
    }
}