using UnityEngine;

namespace WorkerContent
{
    public class WorkerAnimation : MonoBehaviour
    {
        [SerializeField] protected Animator Animator;
        
        public void SetWalkAnimValue(bool value)
        {
            Animator.SetBool("Walk", value);
        }
        
        public void SetCleaningAnimValue(bool value)
        {
            Animator.SetBool("Cleaning", value);
        }
        
        public void SetCalculateAnimValue(bool value)
        {
            Animator.SetBool("Calculate", value);
        }
    }
}