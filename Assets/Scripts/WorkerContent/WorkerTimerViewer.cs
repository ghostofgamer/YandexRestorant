using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WorkerContent
{
    public class WorkerTimerViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private Sprite _workSprite;
        [SerializeField] private Sprite _relaxSprite;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _timeFillImage;

        private WorkerStateType _currentStateType = WorkerStateType.Empty;

        public void UpdateTimerView(float elapsedTime, WorkerStateType stateType, float duration)
        {
            _timerText.text = Mathf.CeilToInt(elapsedTime).ToString("00") + "s";

            if (stateType != _currentStateType)
            {
                _currentStateType = stateType;
                _icon.sprite = stateType == WorkerStateType.Work ? _workSprite : _relaxSprite;
            }

            float fillAmount = elapsedTime / duration;
            _timeFillImage.fillAmount = fillAmount;
        }
    }
}