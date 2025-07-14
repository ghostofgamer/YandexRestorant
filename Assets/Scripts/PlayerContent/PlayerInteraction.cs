using System;
using Enums;
using InputContent;
using Interfaces;
using ItemContent;
using SettingsContent.SoundContent;
using TutorialContent;
using UnityEngine;

namespace PlayerContent
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;

        [SerializeField] private Transform _draggablePosition;
        [SerializeField] private GameObject _throwButton;
        [SerializeField] private PlayerTray _playerTray;
        [SerializeField] private BoxesCounter _boxesCounter;

        private IInteractable _currentInteractable;
        private Vector3 _originalScale;
        private PlayerInput _playerInput;

        public event Action<IInteractable> CurrentDraggerChanger;
        
        public PlayerTray PlayerTray => _playerTray;

        public Draggable CurrentDraggable { get; private set; }

        public Transform DraggablePosition => _draggablePosition;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.ActionEvent += Action;
            _playerInput.ThrowEvent += ThrowItem;
        }

        private void OnDisable()
        {
            _playerInput.ActionEvent -= Action;
            _playerInput.ThrowEvent -= ThrowItem;
        }

        public void SetCurrentInteractableObject(IInteractable iInteractable)
        {
            _currentInteractable = iInteractable;
            CurrentDraggerChanger?.Invoke(_currentInteractable);
        }

        public void Action()
        {
            if (_currentInteractable != null)
                _currentInteractable.Action(this);
        }

        public void SetDraggableObject(Draggable draggable)
        {
            SoundPlayer.Instance.PlayPickUp();
            CurrentDraggable = draggable;
            draggable.transform.SetParent(_draggablePosition);
            draggable.GetComponent<Rigidbody>().isKinematic = true;
            _throwButton.SetActive(true);

            var tutorialObject = draggable.GetComponent<TutorialObject>();

            if (tutorialObject != null && _tutorial.CurrentType == tutorialObject.ItemType)
            {
                switch (tutorialObject.ItemType)
                {
                    case TutorialType.TakeBoxBuns:
                        Debug.Log("типы совпадают");
                        tutorialObject.DeactivateTutorPoint();
                        _boxesCounter.AddBox(draggable.gameObject);
                        _tutorial.SetCurrentTutorialStage(TutorialType.TakeBoxBuns);
                        break;
                    case TutorialType.TakeBoxBurgerPackages:
                        Debug.Log("типы совпадают");
                        tutorialObject.DeactivateTutorPoint();
                        _boxesCounter.AddBox(draggable.gameObject);
                        _tutorial.SetCurrentTutorialStage(TutorialType.TakeBoxBurgerPackages);
                        break;
                    case TutorialType.TakeBoxesOutside:
                        tutorialObject.DeactivateTutorPoint();
                        _boxesCounter.AddBox(draggable.gameObject);
                        _tutorial.SetCurrentTutorialStage(TutorialType.TakeBoxesOutside);
                        break;
                }
            }
        }

        public void SetCurrentDraggable(Draggable draggable)
        {
            CurrentDraggable = draggable;
        }

        public void ThrowItem()
        {
            if (CurrentDraggable == null)
                return;

            SoundPlayer.Instance.PlayThrow();
            CurrentDraggable.Throw();
            CurrentDraggable.GetComponent<Rigidbody>().isKinematic = false;
            CurrentDraggable.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 16f, ForceMode.Impulse);
            ClearDraggableObject();
            _throwButton.SetActive(false);
        }

        public void ClearDraggableObject()
        {
            CurrentDraggable.transform.SetParent(null);
            CurrentDraggable = null;
        }

        public void PutItemShelf()
        {
            _throwButton.SetActive(false);
            ClearDraggableObject();
        }
    }
}