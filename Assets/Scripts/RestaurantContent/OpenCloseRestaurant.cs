using System;
using Enums;
using I2.Loc;
using InteractableContent;
using PlayerContent;
using SettingsContent;
using SettingsContent.SoundContent;
using TMPro;
using TutorialContent;
using UnityEngine;

namespace RestaurantContent
{
    public class OpenCloseRestaurant : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private TMP_Text[] _texts;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Material _openMaterial;
        [SerializeField] private Material _closeMaterial;
        [SerializeField] private Renderer _colorObject;
        [SerializeField] private Color _openColor;
        [SerializeField] private Color _closeColor;
        [SerializeField] private LanguageChanger _languageChanger;

        public bool IsOpened { get; private set; }

        public event Action<bool> OpenedChanged;

        private void OnEnable()
        {
            _interactableObject.OnAction += SetValue;
            _languageChanger.LanguageChanged += ChangeLanguage;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= SetValue;
            _languageChanger.LanguageChanged += ChangeLanguage;
        }

        private void Start()
        {
            Show();
        }

        private void SetValue(PlayerInteraction playerInteraction)
        {
            if ((int)_tutorial.CurrentType < (int)TutorialType.OpenRestaurant)
                return;

            if (_tutorial.CurrentType == TutorialType.OpenRestaurant)
                _tutorial.SetCurrentTutorialStage(TutorialType.OpenRestaurant);

            SoundPlayer.Instance.PlayButtonClick();
            IsOpened = !IsOpened;
            OpenedChanged?.Invoke(IsOpened);
            Show();
        }

        private void Show()
        {
            /*foreach (var text in _texts)
                text.text = IsOpened ? "Open" : "Close";*/

            if (IsOpened)
            {
                // _text.text = "OPEN";
                _text.text = LocalizationManager.GetTermTranslation("OPEN");
                _text.color = _openColor;
                _colorObject.material = _openMaterial;
            }
            else
            {
                // _text.text = "CLOSE";
                _text.text = LocalizationManager.GetTermTranslation("CLOSE");
                _text.color = _closeColor;
                _colorObject.material = _closeMaterial;
            }
        }

        private void ChangeLanguage()
        {
            if (IsOpened)
                _text.text = LocalizationManager.GetTermTranslation("OPEN");
            else
                _text.text = LocalizationManager.GetTermTranslation("CLOSE");
        }
    }
}