using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RewardAdDisableInterLockChanger : MonoBehaviour
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _completedColor;
        [SerializeField] private GameObject _checkObject;
        [SerializeField] private GameObject _lockObject;
        [SerializeField] private int _targetValue;
        [SerializeField] private Image _image;

        public void SetValue(int currentValue)
        {
            _image.color = currentValue >= _targetValue ? _completedColor : _defaultColor;
            _checkObject.SetActive(currentValue >= _targetValue);
            _lockObject.SetActive(currentValue < _targetValue);
        }
    }
}