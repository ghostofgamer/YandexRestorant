using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enozone
{
    public class AnimationToggle : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        private void OnMouseDown()
        {
            animator.SetTrigger("toggle");
        }
    }
}