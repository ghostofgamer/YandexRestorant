using UnityEngine;

namespace AttentionHintContent
{
    public class AttentionHintActivator : MonoBehaviour
    {
        [SerializeField] private AttentionHintViewer _attentionHintViewer;
        
        private static AttentionHintActivator _instance;
        
        public static AttentionHintActivator Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("AttentionHintActivator");
                    _instance = go.AddComponent<AttentionHintActivator>();
                    DontDestroyOnLoad(go);
                }
                
                return _instance;
            }
        }
        
       private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(this.gameObject);
            else
                _instance = this;
        }
       
        public void ShowHint(string message)
        {
            _attentionHintViewer.ShowAttentionHint(message);
        }
    }
}
