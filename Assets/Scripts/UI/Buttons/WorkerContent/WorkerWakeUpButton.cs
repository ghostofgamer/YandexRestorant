using UnityEngine;
using WorkerContent.WorkerWakeUpContent;

namespace UI.Buttons.WorkerContent
{
    public class WorkerWakeUpButton : AbstractButton
    {
        [SerializeField] private WorkerAwakening _workerAwakening;
        [SerializeField] private bool _isAdsButton;

        public override void OnClick() => _workerAwakening.Wake(_isAdsButton);
    }
}