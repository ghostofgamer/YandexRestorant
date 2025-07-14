using System;
using System.Collections;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerToolMover : MonoBehaviour
    {
        [SerializeField] private FryerTool _fryerTool;
        
        public float moveDistance = 0.15f; // Расстояние, на которое объект будет двигаться вниз
        public float moveDuration = 1.0f; // Длительность движения в одну сторону

        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private bool _isMovingDown = true;

        private void Start()
        {
            _startPosition = transform.position;
            _targetPosition = _startPosition - new Vector3(0, moveDistance, 0);
        }

        public void MoveFrying()
        {
            StartCoroutine(MoveObject());
        }

        IEnumerator MoveObject()
        {
            yield return StartCoroutine(MoveToPosition(_targetPosition, moveDuration));
            yield return new WaitForSeconds(1.5f);
            _fryerTool.ActivateWellItems();
            yield return StartCoroutine(MoveToPosition(_startPosition, moveDuration));
        }

        IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
        {
            float elapsedTime = 0;
            Vector3 startingPosition = transform.position;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }
    }
}