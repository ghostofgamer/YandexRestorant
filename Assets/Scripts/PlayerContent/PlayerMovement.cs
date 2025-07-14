using Enums;
using TutorialContent;
using UI.Screens.TutorialScreens;
using UnityEngine;

namespace PlayerContent
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private TutorialType _tutorialType;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private MoveScreen _moveScreen;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _footstepSounds;
        [SerializeField] private float _stepInterval = 0.5f;
        
        private float _gravity = -9.81f;
        private Vector3 _moveDirection;
        private CharacterController _controller;
        private float _verticalVelocity;
        private float _rotationX = 0;
        private float _nextStepTime = 0f;
        
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void MovePlayer(float horizontal,float vertical)
        {
            if (_controller.enabled == false)
                return;
            
            if ((int)_tutorial.CurrentType < (int)_tutorialType)
            {
                Debug.Log("(int)_tutorial.CurrentType < (int)_tutorialType");
                return;
            }
            
            _moveDirection = new Vector3(horizontal, 0, vertical);
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= _moveSpeed;

            if (_controller.isGrounded && _verticalVelocity < 0)
                _verticalVelocity = -2f;
            else
                _verticalVelocity += _gravity * Time.deltaTime;

            _moveDirection.y = _verticalVelocity;
            _controller.Move(_moveDirection * Time.deltaTime);
            
            if (_moveDirection.magnitude > 3f && Time.time >= _nextStepTime)
            {
                if ((int)_tutorial.CurrentType == (int)_tutorialType)
                {
                    Debug.Log("Выполнил Current этап тутора");
                    _moveScreen.CloseScreen();
                    _tutorial.SetCurrentTutorialStage(_tutorialType);
                }
                
                
                PlayFootstepSound();
                _nextStepTime = Time.time + _stepInterval;
            }
        }
        
        private void PlayFootstepSound()
        {
            if (_footstepSounds.Length == 0)
                return;
            
            int randomIndex = Random.Range(0, _footstepSounds.Length);
            AudioClip footstepSound = _footstepSounds[randomIndex];
            _audioSource.PlayOneShot(footstepSound);
        }
    }
}