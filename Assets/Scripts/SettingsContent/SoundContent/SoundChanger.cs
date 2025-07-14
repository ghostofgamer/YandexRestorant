using UnityEngine;

namespace SettingsContent
{
    public class SoundChanger : MonoBehaviour
    {
        [SerializeField] private ToggleSwitch _soundToggleSwitch;
        [SerializeField] private ToggleSwitch _sfxToggleSwitch;

        private void Start()
        {
            if (_soundToggleSwitch != null)
            {
                _soundToggleSwitch.onToggleOn.AddListener(() => ChangeValueSound(true));
                _soundToggleSwitch.onToggleOff.AddListener(() => ChangeValueSound(false));
                _soundToggleSwitch.SetStateAndStartAnimation(Sound.Instance.IsSoundOn());
            }

            if (_sfxToggleSwitch != null)
            {
                _sfxToggleSwitch.onToggleOn.AddListener(() => ChangeValueSFX(true));
                _sfxToggleSwitch.onToggleOff.AddListener(() => ChangeValueSFX(false));
                _sfxToggleSwitch.SetStateAndStartAnimation(Sound.Instance.IsSFXOn());
            }
        }

        public void ChangeValueSound(bool enabled)
        {
            Sound.Instance.SetSound(enabled);
        }

        public void ChangeValueSFX(bool enabled)
        {
            Sound.Instance.SetSFX(enabled);
        }
    }
}