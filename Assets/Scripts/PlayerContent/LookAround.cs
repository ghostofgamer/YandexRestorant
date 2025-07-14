using System.Collections;
using Enums;
using InputContent;
using SettingsContent;
using TutorialContent;
using UI.Screens.TutorialScreens;
using UnityEngine;

namespace PlayerContent
{
    public class LookAround : MonoBehaviour
    {
        [SerializeField] private TutorialType _tutorialType;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private LookAroundScreen _lookAroundScreen;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private float _lookSpeed;
        [SerializeField] private SensitivitySettings _sensitivitySettings;
        [SerializeField] private float _smoothTime = 0.1f;
        [SerializeField] private float _targetLookDuration = 1f;
        [SerializeField] private AnimationCurve _lookCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private PlayerRotator _rotator;

        private float _verticalLookLimit = 80f;
        private float _rotationX = 0;
        private float _currentRotationX;
        private float _currentRotationY;
        private float _rotationXVelocity;
        private float _rotationYVelocity;
        private Coroutine _lookAtCoroutine;

        public float LookSpeed => _lookSpeed;

        public void Looking(float x, float y)
        {
            if ((int)_tutorial.CurrentType < (int)_tutorialType)
            {
                Debug.Log("(int)_tutorial.CurrentType < (int)_tutorialType");
                return;
            }

            if ((int)_tutorial.CurrentType == (int)_tutorialType)
            {
                Debug.Log("Выполнил Current этап тутора");
                _lookAroundScreen.CloseScreen();
                _tutorial.SetCurrentTutorialStage(_tutorialType);
            }

            float sensitivity = _sensitivitySettings.SensitivityMouse / 100f; // Нормализация значения чувствительности
            float effectiveLookSpeed = _lookSpeed * sensitivity;

            _rotationX -= y * effectiveLookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -_verticalLookLimit, _verticalLookLimit);
            _currentRotationX = Mathf.SmoothDamp(_currentRotationX, _rotationX, ref _rotationXVelocity, _smoothTime);
            float rotationY = x * effectiveLookSpeed;
            _currentRotationY = Mathf.SmoothDamp(_currentRotationY, rotationY, ref _rotationYVelocity, _smoothTime);

            transform.localRotation = Quaternion.Euler(_currentRotationX, 0, 0);
            _playerBody.Rotate(Vector3.up * _currentRotationY);
        }
        
        
        public void LookAtPosition(Vector3 worldPosition)
        {
            if (_lookAtCoroutine != null)
            {
                StopCoroutine(_lookAtCoroutine);
            }

            _rotator.SetValue(false);
            GameObject tempTarget = new GameObject("TempLookTarget");
            tempTarget.transform.position = worldPosition;
            _lookAtCoroutine = StartCoroutine(LookAtTargetCoroutine(tempTarget.transform, () => Destroy(tempTarget)));
        }
        
        private IEnumerator LookAtTargetCoroutine(Transform target, System.Action onComplete = null)
        {
            Vector3 startForward = _playerBody.forward;
            float startCameraXRotation = NormalizeAngle(transform.localEulerAngles.x);
            float startBodyYRotation = NormalizeAngle(_playerBody.eulerAngles.y);

            // Calculate target rotations
            Vector3 toTarget = target.position - _playerBody.position;
            Vector3 horizontalDirection = new Vector3(toTarget.x, 0, toTarget.z).normalized;
        
            // Body rotation (horizontal)
            Quaternion targetBodyRotation = Quaternion.LookRotation(horizontalDirection);
            float targetBodyY = NormalizeAngle(targetBodyRotation.eulerAngles.y);

            // Camera rotation (vertical)
            Vector3 cameraToTarget = target.position - transform.position;
            float targetCameraX = Mathf.Clamp(
                Mathf.Atan2(-cameraToTarget.y, cameraToTarget.magnitude) * Mathf.Rad2Deg,
                -_verticalLookLimit,
                _verticalLookLimit
            );

            float elapsedTime = 0f;

            while (elapsedTime < _targetLookDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = _lookCurve.Evaluate(elapsedTime / _targetLookDuration);

                // Interpolate body rotation
                float currentBodyY = Mathf.LerpAngle(startBodyYRotation, targetBodyY, t);
            
                // Interpolate camera rotation
                float currentCameraX = Mathf.LerpAngle(startCameraXRotation, targetCameraX, t);

                // Apply rotations
                _playerBody.rotation = Quaternion.Euler(0, currentBodyY, 0);
                transform.localRotation = Quaternion.Euler(currentCameraX, 0, 0);

                // Update state variables
                _rotationX = currentCameraX;
                _currentRotationX = currentCameraX;
                _currentRotationY = currentBodyY - startBodyYRotation;

                yield return null;
            }
            
            
            ResetStateAfterAnimation();
            _rotator.SetValue(true);
            onComplete?.Invoke();
        }
        
        private float NormalizeAngle(float angle)
        {
            while (angle > 180) angle -= 360;
            while (angle < -180) angle += 360;
            return angle;
        }
        
        private void ResetStateAfterAnimation()
        {
            // Принудительная синхронизация с текущим состоянием
            _rotationX = NormalizeAngle(transform.localEulerAngles.x);
            _currentRotationX = _rotationX;
            _currentRotationY = 0f;
            _rotationXVelocity = 0f;
            _rotationYVelocity = 0f;
            
            // Сброс любых накопленных значений
            _playerBody.rotation = Quaternion.Euler(0, _playerBody.eulerAngles.y, 0);
            transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        }
    }
}