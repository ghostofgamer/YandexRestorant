using UnityEngine;

namespace DoorsContent
{
    public class DoorAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void OpeningAnim()
        {
            _animator.SetBool("Opening", true);
        }

        public void ClosingAnim()
        {
            _animator.SetBool("Closing", true);
        }
    }
}