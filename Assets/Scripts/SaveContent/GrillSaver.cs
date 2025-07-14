using System;
using KitchenEquipmentContent;
using UnityEngine;

namespace SaveContent
{
    public class GrillSaver : MonoBehaviour
    {
        private Grill _grill;
        private int _rawCutletActiveValue;
        private int _wellCutletActiveValue;

        private void Awake()
        {
            _grill = GetComponent<Grill>();
        }

        private void OnEnable()
        {
            _grill.ValueActiveItemsChanged += SaveDate;
        }

        private void OnDisable()
        {
            _grill.ValueActiveItemsChanged -= SaveDate;
        }

        private void SaveDate(int rawValue, int wellValue)
        {
            PlayerPrefs.SetInt("RawCutletGrill", rawValue);
            PlayerPrefs.SetInt("WellCutletGrill", wellValue);
        }
    }
}