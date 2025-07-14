using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MysteryGiftContent
{
    public class MysteryGiftActivator : MonoBehaviour
    {
        [SerializeField] private List<Transform> _positions;
        [SerializeField] private MysteryGift _mysteryGift;

        private float _elapsedTime;
        private float _duration = 10f;
        private bool _isStopped = false;
        private bool _isPaused = false;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(10f);
        private float _activationDuration = 180f;
        private float _deactivationDuration = 60f;
        
        private void Start()
        {
            if(_coroutine!=null)
                StopCoroutine(_coroutine);
            
            _coroutine = StartCoroutine(ActivateObjectPeriodically());
        }

        private void OnEnable()
        {
            _mysteryGift.BoxActivation += ActivatePaused;
            _mysteryGift.BoxDeactivation += DeactivatePaused;
        }

        private void OnDisable()
        {
            _mysteryGift.BoxActivation -= ActivatePaused;
            _mysteryGift.BoxDeactivation -= DeactivatePaused;
        }

        private IEnumerator ActivateObjectPeriodically()
        {
            while (!_isStopped)
            {
                if (!_isPaused)
                {
                    yield return new WaitForSeconds(_activationDuration);
                    
                    if (!_isStopped && !_isPaused)
                    {
                        int posIndex = Random.Range(0, _positions.Count);
                        InitPosition(_positions[posIndex]);

                        yield return new WaitForSeconds(_deactivationDuration);

                        if (!_isStopped && !_isPaused)
                        {
                            _mysteryGift.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }
        
        public void SetStopped(bool value)
        {
            _isStopped = value;
            
            if (_isStopped && _coroutine != null)
                StopCoroutine(_coroutine);
        }
        
        private void ActivatePaused()
        {
            _isPaused = true;
        }
        
        private void DeactivatePaused()
        {
            _isPaused = false;
        }

        private void InitPosition(Transform transform)
        {
            _mysteryGift.transform.position = transform.position;
            _mysteryGift.transform.rotation = transform.rotation;
            _mysteryGift.gameObject.SetActive(true);
        }
    }
}