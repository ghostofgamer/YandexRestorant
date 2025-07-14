using UnityEngine;

namespace TaskContent.FortuneTaskContent
{
    public class GlowRotation : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        
        private void Update()
        {
            transform.Rotate(Vector3.forward, _speed * Time.deltaTime);
        }
    }
}