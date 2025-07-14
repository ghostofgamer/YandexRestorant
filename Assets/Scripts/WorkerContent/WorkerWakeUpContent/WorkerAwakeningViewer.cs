using Enums;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace WorkerContent.WorkerWakeUpContent
{
    public class WorkerAwakeningViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _stateTypeText;
        [SerializeField] private TMP_Text _timerText;

        public void ShowInfo(WorkerStateType type, float time)
        {
            if (type == WorkerStateType.Relax)
            {
                _stateTypeText.text = $"{LocalizationManager.GetTermTranslation("Rest")}";
            }
            else if (type == WorkerStateType.Work)
            {
                _stateTypeText.text = $"{LocalizationManager.GetTermTranslation("Work")}";
            }

            _timerText.text = time.ToString("F0");
        }
    }
}