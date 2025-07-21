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

        public virtual void OpenScreen()
        {
            gameObject.SetActive(true);
            // InterstitialActivator.Instance.ShowAd();
            InterstitialActivator.Instance.ShowAd(_breakADCooldown);
            
            if (_playerInput != null)
                _playerInput.enabled = false;

            if (_cursorActivator != null)
                _cursorActivator.SetValueCursor(true);
        }

        public virtual void CloseScreen()
        {
            // InterstitialActivator.Instance.ShowAd();
            gameObject.SetActive(false);

            if (_playerInput != null)
                _playerInput.enabled = true;

            if (_cursorActivator != null)
                _cursorActivator.SetValueCursor(false);
        }
    }
}