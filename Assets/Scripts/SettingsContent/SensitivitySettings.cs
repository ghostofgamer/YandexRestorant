using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsContent
{
    public class SensitivitySettings : MonoBehaviour
    {
        private const string Sensitivity = "Sensitivity";
        
        [field: SerializeField] public float SensitivityMouse { get; private set; } = 100f;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private Slider _sensitivitySlider;
        
        private float _minSensitivity = 0.5f;
        private float _maxSensitivity = 600f;
        private float _defaultSensitivity = 300f;
        private float _min = 0.5f;
        private float _max = 5f;

        private void Start()
        {
            float currentSensitivity = PlayerPrefs.GetFloat(Sensitivity, _defaultSensitivity);

            _sensitivitySlider.minValue = 0.6f;
            _sensitivitySlider.maxValue = _max;
            _sensitivitySlider.value = MapValue(currentSensitivity, _minSensitivity, _maxSensitivity, _min, _max);
            _valueText.text = _sensitivitySlider.value.ToString("F1");
            _sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
            SensitivityMouse = currentSensitivity;
        }

        private void OnSensitivityChanged(float value)
        {
            SensitivityMouse = MapValue(value, _min, _max, _minSensitivity, _maxSensitivity);
            _valueText.text = value.ToString("F1");
            PlayerPrefs.SetFloat(Sensitivity, SensitivityMouse);
            PlayerPrefs.Save();
        }

        private float MapValue(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            if (value < fromMin) value = fromMin;
            if (value > fromMax) value = fromMax;

            float mappedValue = (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
            return mappedValue;
        }
    }
}