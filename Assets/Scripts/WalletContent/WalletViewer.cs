using System;
using TMPro;
using UnityEngine;

namespace WalletContent
{
    public class WalletViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentDollarValue;
        [SerializeField] private Wallet _wallet;

        private void OnEnable()
        {
            _wallet.DollarValueChanged += ShowDollarValue;
        }

        private void OnDisable()
        {
            _wallet.DollarValueChanged -= ShowDollarValue;
        }
        
        private void ShowDollarValue(DollarValue dollarValue)
        {
            _currentDollarValue.text = $"{dollarValue.Dollars}.{dollarValue.Cents:D2}";
        }
    }
}