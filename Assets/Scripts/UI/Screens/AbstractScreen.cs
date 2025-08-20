using ADSContent;
using InputContent;
using UnityEngine;

namespace UI.Screens
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CursorActivator _cursorActivator;
        [SerializeField] private bool _breakADCooldown;
        [SerializeField] private WindowsCounter _windowsCounter;

        public virtual void OpenScreen()
        {
            gameObject.SetActive(true);
            // InterstitialActivator.Instance.ShowAd();
            InterstitialActivator.Instance.ShowAd(_breakADCooldown);

            if (_windowsCounter != null)
            {
                Debug.Log("Открывается окно " + this.name);
                // _windowsCounter.IncreaseValue();
                _windowsCounter.TryAddWindow(this);
            }

            if (_playerInput != null)
                _playerInput.enabled = false;

            if (_cursorActivator != null)
                _cursorActivator.SetValueCursor(true);
        }

        public virtual void CloseScreen()
        {
            // InterstitialActivator.Instance.ShowAd();
            gameObject.SetActive(false);

            if (_windowsCounter != null)
            {
                Debug.Log("Закрывается окно " + this.name);
                // _windowsCounter.DecreaseValue();
                _windowsCounter.TryRemoveWindow(this);
                
                if (_playerInput != null && _windowsCounter.CurrentValue <= 0)
                    _playerInput.enabled = true;
            }
            else
            {
                if (_playerInput != null)
                    _playerInput.enabled = true;
            }
            
            /*if (_playerInput != null)
                _playerInput.enabled = true;*/

            if (_cursorActivator != null)
                _cursorActivator.SetValueCursor(false);
        }
    }
}