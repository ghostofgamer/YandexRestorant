using System;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace UI.Screens.AdsScreens
{
    public class RemoveAdScreen : AbstractScreen
    {
        [SerializeField] private TMP_Text _descriptionText;

        private void OnEnable()
        {
            _descriptionText.text =
                $"{LocalizationManager.GetTermTranslation("NO ADS")}\n<color=yellow>{LocalizationManager.GetTermTranslation("FOREVER")}</color>";
        }

        private void Start()
        {
            // _descriptionText.text = $"NO ADS\n<color=yellow>FOREVER</color>";
            _descriptionText.text =
                $"{LocalizationManager.GetTermTranslation("NO ADS")}\n<color=yellow>{LocalizationManager.GetTermTranslation("FOREVER")}</color>";
        }
    }
}