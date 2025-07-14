using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SettingsContent
{
    public class Sound : MonoBehaviour
    {
        private const string SoundKey = "SoundEnabled";
        private const string SFXKey = "SFXEnabled";

        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField] private string _volumeParameter = "Sound";
        [SerializeField] private string _sfxParameter = "SFX";

        private bool _isSoundOn = true;
        private bool _isSFXOn = true;
        private float _valueEnabled = 0f;
        private float _valueDisabled = -80f;

        public static Sound Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadSettings();
        }

        public void SetSound(bool enabled)
        {
            _audioMixerGroup.audioMixer.SetFloat(_volumeParameter, enabled ? _valueEnabled : _valueDisabled);
            _isSoundOn = enabled;
            PlayerPrefs.SetInt(SoundKey, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void SetSFX(bool enabled)
        {
            _audioMixerGroup.audioMixer.SetFloat(_sfxParameter, enabled ? 0f : -80f);
            _isSFXOn = enabled;
            PlayerPrefs.SetInt(SFXKey, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        public bool IsSoundOn()
        {
            return _isSoundOn;
        }

        public bool IsSFXOn()
        {
            return _isSFXOn;
        }

        private void LoadSettings()
        {
            _isSoundOn = PlayerPrefs.GetInt(SoundKey, 1) == 1;
            _isSFXOn = PlayerPrefs.GetInt(SFXKey, 1) == 1;

            SetSound(_isSoundOn);
            SetSFX(_isSFXOn);
        }
    }
}
