using InputContent;
using PlayerContent;
using UnityEngine;

namespace TutorialContent
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private float _rotationSpeed = 5.0f;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private LookAround _lookAround;
        [SerializeField] private GameObject _rotator;

        private Coroutine _rotationCoroutine;
        private Coroutine _lookCoroutine;
        private Quaternion _savedRotation;

        public void RotateToTarget(Transform target)
        {
            _lookAround.LookAtPosition(target.position);
        }
        
        public void SetValue(bool value)
        {
            _lookAround.enabled = value;
            _playerMovement.enabled = value;
            _playerInput.enabled = value;
        }
    }
}