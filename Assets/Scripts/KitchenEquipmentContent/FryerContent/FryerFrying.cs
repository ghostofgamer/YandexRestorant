using System;
using System.Collections;
using System.Collections.Generic;
using AttentionHintContent;
using Enums;
using I2.Loc;
using SettingsContent.SoundContent;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerFrying : MonoBehaviour
    {
        [SerializeField] private AssemblyFryerTable _assemblyFryerTable;
        [SerializeField] private FryerContainer[] _fryerContainers;
        [SerializeField] private FryerPacking _fryerPacking;
        [SerializeField] private Collider _collider;
        [SerializeField] private GameObject _progressFryUI;
        [SerializeField] private TMP_Text _fryText;
        [SerializeField] private Image _fillImage;
        [SerializeField] private float _fryTime = 3f;
        [SerializeField] private AudioSource _audioSource;

        [FormerlySerializedAs("_effects")] [SerializeField]
        private GameObject[] _effectsFrying;

        private Coroutine _coroutine;
        private List<FryerTool> _fryerTools = new List<FryerTool>();

        public event Action FryCompleted;

        public void Fry()
        {
            Debug.Log("!!!_fryerPacking.GetFullTools() " + _fryerPacking.GetFullTools());

            if (_fryerPacking.GetFullTools() <= 0)
            {
                AttentionHintActivator.Instance.ShowHint(LocalizationManager.GetTermTranslation("Nothing to cook"));
                Debug.Log("нечего жарить ");
                return;
            }

            int value = 0;

            foreach (var fryerContainer in _fryerContainers)
            {
                Debug.Log("В контейнере пустых мест " + fryerContainer.GetInactiveValue() + " ??? " +
                          fryerContainer.ItemType);

                if (fryerContainer.GetInactiveValue() > 0)
                {
                    FryerTool fryerTool = _fryerPacking.GetCompatibleFryerTool(fryerContainer.ItemType);

                    if (fryerTool != null)
                    {
                        int activeValue = fryerTool.GetCountActiveItems();
                        Debug.Log("В контейнере активно " + activeValue);

                        if (activeValue > 0)
                            value++;
                    }
                }
            }

            Debug.Log("!!!value " + value);

            if (value <= 0)
            {
                Debug.Log("нету мест в контейнере");
                return;
            }


            Debug.Log("Жарить");

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartFry());
        }

        private IEnumerator StartFry()
        {
            _collider.enabled = false;

            foreach (var fryerContainer in _fryerContainers)
            {
                Debug.Log("В контейнере пустых мест " + fryerContainer.GetInactiveValue() + " ??? " +
                          fryerContainer.ItemType);

                if (fryerContainer.GetInactiveValue() > 0)
                    SelectFryerTool(fryerContainer.ItemType);
            }

            yield return new WaitForSeconds(1f);
            _audioSource.Play();
            _progressFryUI.SetActive(true);
            // _fryText.text = "Fry <color=yellow>Raw</color>";
            _fryText.text = $"{LocalizationManager.GetTermTranslation("Fry")} <color=yellow>{LocalizationManager.GetTermTranslation("Raw")}</color>";
            _fillImage.fillAmount = 0f;

            float elapsedTime = 0f;

            while (elapsedTime < _fryTime)
            {
                elapsedTime += Time.deltaTime;
                _fillImage.fillAmount = elapsedTime / _fryTime;
                yield return null;
            }

            _audioSource.Stop();
            // _fryText.text = "Fry <color=green>Well</color>";
            _fryText.text = $"{LocalizationManager.GetTermTranslation("Fry")} <color=green>{LocalizationManager.GetTermTranslation("Well")}</color>";
            SoundPlayer.Instance.PlayGrillWell();

            yield return new WaitForSeconds(0.3f);
            _progressFryUI.SetActive(false);
            Debug.Log("закончили жарить ");
            FryCompleted?.Invoke();
            _collider.enabled = true;

            foreach (var effectObject in _effectsFrying)
                effectObject.SetActive(false);
        }

        private void SelectFryerTool(ItemType itemType)
        {
            FryerTool fryerTool = _fryerPacking.GetCompatibleFryerTool(itemType);

            if (fryerTool != null)
            {
                int activeValue = fryerTool.GetCountActiveItems();
                Debug.Log("В контейнере активно " + activeValue);

                if (activeValue > 0)
                    fryerTool.MoveFrying();
            }
        }
    }
}