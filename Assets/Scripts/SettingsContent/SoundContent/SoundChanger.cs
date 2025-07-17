using UnityEngine;
using UnityEngine.UI;

namespace SettingsContent.SoundContent
{
    public class SoundChanger : MonoBehaviour
    {
        [SerializeField] private Toggle _soundToggleSwitch1;
        [SerializeField] private Toggle _sfxToggleSwitch1;
        [SerializeField] private GameObject[] _soundImages;
        [SerializeField] private GameObject[] _musicImages;
        [SerializeField]private Image _backgroundImageSFX;
        [SerializeField]private Image _backgroundImageSound;
        [SerializeField]private Color _activeColor;
        [SerializeField]private Color _inactiveColor;

        private void OnEnable()
        {
            if (_soundToggleSwitch1 != null)
                _soundToggleSwitch1.onValueChanged.AddListener(ChangeValueSound);

            if (_sfxToggleSwitch1 != null)
                _sfxToggleSwitch1.onValueChanged.AddListener(ChangeValueSFX);
        }

        private void OnDisable()
        {
            if (_soundToggleSwitch1 != null)
                _soundToggleSwitch1.onValueChanged.RemoveListener(ChangeValueSound);

            if (_sfxToggleSwitch1 != null)
                _sfxToggleSwitch1.onValueChanged.RemoveListener(ChangeValueSFX);
        }

        public void Init(bool soundOn, bool sfxOn)
        {
            SetSoundValue(soundOn);
            SetSFXValue(sfxOn);
        }

        private void ChangeValueSound(bool enabled)
        {
            if (SoundPlayer.Instance != null)
                SoundPlayer.Instance.PlayButtonClick();

            Sound.Instance.SetSound(enabled);
            SetSoundValue(enabled);
        }

        private void ChangeValueSFX(bool enabled)
        {
            if (SoundPlayer.Instance != null)
                SoundPlayer.Instance.PlayButtonClick();

            Sound.Instance.SetSFX(enabled);
            SetSFXValue(enabled);
        }

        private void SetSFXValue(bool sfx)
        {
            _backgroundImageSFX.color = sfx ? _activeColor : _inactiveColor;
            _sfxToggleSwitch1.isOn = sfx;
            _soundImages[0].SetActive(sfx);
            _soundImages[1].SetActive(!sfx);
        }

        private void SetSoundValue(bool sound)
        {
            _backgroundImageSound.color = sound ? _activeColor : _inactiveColor;
            _soundToggleSwitch1.isOn = sound;
            _musicImages[0].SetActive(sound);
            _musicImages[1].SetActive(!sound);
        }
    }
}