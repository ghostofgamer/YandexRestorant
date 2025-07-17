using ADSContent;
using InputContent;
using UnityEngine;

namespace UI.Screens
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CursorActivator _cursorActivator;

        public virtual void OpenScreen()
        {
            gameObject.SetActive(true);
            
            if (_playerInput != null)
                _playerInput.enabled = false;

            if (_cursorActivator != null)
                _cursorActivator.SetValueCursor(true);
        }

        public virtual void CloseScreen()
        {
            InterstitialActivator.Instance.ShowAd();
            gameObject.SetActive(false);

            if (_playerInput != null)
                _playerInput.enabled = true;

            if (_cursorActivator != null)
                _cursorActivator.SetValueCursor(false);
        }
    }
}