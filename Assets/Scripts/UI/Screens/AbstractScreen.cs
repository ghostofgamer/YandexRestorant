using ADSContent;
using InputContent;
using UnityEngine;

namespace UI.Screens
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        
        public virtual void OpenScreen()
        {
            gameObject.SetActive(true);

            if (_playerInput != null)
                _playerInput.enabled = false;
        }

        public virtual void CloseScreen()
        {
            InterstitialActivator.Instance.ShowAd();
            gameObject.SetActive(false);

            if (_playerInput != null)
                _playerInput.enabled = true;
        }
    }
}