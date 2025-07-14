using System;
using System.Collections;
using Enums;
using I2.Loc;
using SettingsContent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialContent
{
    public class TutorDescriptionUI : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _completedImage;
        [SerializeField] private Image _fillProgrees;
        [SerializeField] private TMP_Text _progressPrecentValueText;
        [SerializeField] private LanguageChanger _languageChanger;
        
        private Coroutine _coroutine;
        private bool _isFirstStage = true;
        private bool _isOpen;
        private int _maxTutorialSteps;
        private int _currentPercent;
        private string _currentTerm;
        
        public event Action TutorialCompleted;

        private void Awake()
        {
            _maxTutorialSteps = System.Enum.GetValues(typeof(TutorialType)).Length - 1;
        }

        private void OnEnable()
        {
            _languageChanger.LanguageChanged += LanguageChange;
            _languageChanger.LanguageChanged += ChangeLanguage;
        }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= LanguageChange;
            _languageChanger.LanguageChanged -= ChangeLanguage;
        }

        public void StartStage(string text, int indexState)
        {
            UpdateProgressVisual(indexState);

            if (!_isOpen)
            {
                if (_isFirstStage)
                {
                    _animator.SetBool("Open", true);
                    _isFirstStage = false;
                    StartNewStage(text);
                }
                else
                {
                    MoveUI(text);
                }
            }
            else
            {
                MoveUI(text);
            }

            _isOpen = true;
        }

        private void StartNewStage(string text)
        {
            _completedImage.gameObject.SetActive(false);
            _descriptionText.gameObject.SetActive(true);
            _currentTerm = text;
            // _descriptionText.text = text;
            _descriptionText.text = LocalizationManager.GetTermTranslation(_currentTerm);
        }

        private void ChangeLanguage()
        {
            _descriptionText.text = LocalizationManager.GetTermTranslation(_currentTerm);
        }

        private void CompleteStage()
        {
            _completedImage.gameObject.SetActive(true);
            _descriptionText.gameObject.SetActive(false);
        }

        private void MoveUI(string text)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartMoveUI(text));
        }

        private IEnumerator StartMoveUI(string text)
        {
            CompleteStage();
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
            yield return new WaitForSeconds(1f);
            StartNewStage(text);
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", true);
        }

        public void StartCompleted(string text, int currentStage)
        {
            StartCoroutine(StartMoveUICompleted(text, currentStage));
        }

        private IEnumerator StartMoveUICompleted(string text, int currentStage)
        {
            UpdateProgressVisual(currentStage);
            CompleteStage();
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
            yield return new WaitForSeconds(1f);
            StartNewStage(text);
            _animator.SetBool("Close", false);
            _animator.SetBool("Open", true);
            yield return new WaitForSeconds(1f);
            TutorialCompleted?.Invoke();
            yield return new WaitForSeconds(2f);
            CompleteStage();
            _animator.SetBool("Open", false);
            _animator.SetBool("Close", true);
        }

        private void UpdateProgressVisual(int currentState)
        {
            if (_fillProgrees == null)
                return;

            float progress = CalculateFillAmount(currentState);
            int percentage = Mathf.RoundToInt(progress * 100);
            _currentPercent = percentage;
            _progressPrecentValueText.text = $"{LocalizationManager.GetTermTranslation("Tutorial progress")}:{percentage}%";
            _fillProgrees.fillAmount = Mathf.Clamp01(progress);
        }

        private float CalculateFillAmount(int currentValue)
        {
            return Mathf.Clamp01((float)currentValue / _maxTutorialSteps);
        }

        private void  LanguageChange()
        {
            _progressPrecentValueText.text = $"{LocalizationManager.GetTermTranslation("Tutorial progress")}:{_currentPercent}%";
        }
    }
}