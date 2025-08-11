using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public abstract class AbstractButton : MonoBehaviour
    {
        protected Button Button;
        
        private void Awake()
        {
            Button = GetComponent<Button>();
        }

        protected virtual void OnEnable()
        {
            Button.onClick.AddListener(OnClick); 
        }

        protected virtual void OnDisable()
        {
            Button.onClick.RemoveListener(OnClick); 
        }

        public abstract void OnClick();
    }
}