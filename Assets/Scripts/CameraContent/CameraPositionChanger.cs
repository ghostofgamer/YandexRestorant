using InputContent;
using UnityEngine;

namespace CameraContent
{
   public class CameraPositionChanger : MonoBehaviour
   {
      [SerializeField] private Camera _camera;
      [SerializeField] private PlayerInput _playerInput;
      [SerializeField] private Transform _defaultParent;
      [SerializeField] private CharacterController _characterController;
      
      private Transform _defaultPosition;
      private Vector3 _default; 

      private void Start()
      {
         _defaultPosition = _camera.transform;
         _default = _camera.transform.localPosition;
      }

      public void ChangePosition(Transform position)
      {
         _playerInput.enabled = false;
         _characterController.enabled = false;
         _camera.transform.parent = position;
         _camera.transform.localPosition = Vector3.zero;
         _camera.transform.localRotation = Quaternion.identity;
      }

      public void ReturnDefaultPosition()
      {
         _camera.transform.parent = _defaultParent;
         _characterController.enabled = true;
         _camera.transform.localPosition = _default;
         _camera.transform.localRotation = Quaternion.identity;
         _playerInput.enabled = true;
      }
   }
}