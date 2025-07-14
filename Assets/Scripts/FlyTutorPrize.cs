using TutorialContent;
using UnityEngine;

public class FlyTutorPrize : MonoBehaviour
{
    [SerializeField] private TutorDescriptionUI _tutorDescriptionUI;
    [SerializeField] private Animator _animator;
    
    private void OnEnable()
    {
        _tutorDescriptionUI.TutorialCompleted += ActivateFly;
    }

    private void OnDisable()
    {
        _tutorDescriptionUI.TutorialCompleted -= ActivateFly;
    }

    private void ActivateFly()
    {
        _animator.enabled = true;
    }
}