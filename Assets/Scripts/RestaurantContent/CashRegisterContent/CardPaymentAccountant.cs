using Enums;
using SettingsContent.SoundContent;
using TMPro;
using TutorialContent;
using UnityEngine;
using WalletContent;

namespace RestaurantContent.CashRegisterContent
{
    public class CardPaymentAccountant : MonoBehaviour
    {
        private const int MaxInputLength = 10;

        [SerializeField] private TMP_Text _inputText;
        [SerializeField] private CashRegister _cashRegister;
        
        public DollarValue PriceOrder { get; set; }

        public string InputString { get; private set; } = string.Empty;

        private void Start()
        {
            InputString = string.Empty;
            UpdateInputText();
        }

        public void Init(DollarValue price)
        {
            PriceOrder = price;
        }

        public void OnDigitButtonClick(int digit)
        {
            if (InputString.Length < MaxInputLength)
            {
                InputString += digit.ToString();
                UpdateInputText();
            }
        }

        public void OnDotButtonClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            if (!InputString.Contains(".") && InputString.Length < MaxInputLength)
            {
                if (string.IsNullOrEmpty(InputString))
                {
                    InputString = "0.";
                }
                else
                {
                    InputString += ".";
                }
                
                UpdateInputText();
            }
        }

        public void OnAcceptButtonClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            if (string.IsNullOrEmpty(InputString))
            {
                Debug.Log("Введите сумму.");
                return;
            }

            if (!InputString.Contains("."))
                InputString += ".00";
            else if (InputString.Split('.')[1].Length == 1)
                InputString += "0";

            string[] parts = InputString.Split('.');
            int dollars = int.Parse(parts[0]);
            int cents = int.Parse(parts[1]);

            DollarValue inputValue = new DollarValue(dollars, cents);

            if (inputValue.Dollars == PriceOrder.Dollars && inputValue.Cents == PriceOrder.Cents)
            {
                Debug.Log("Сумма совпадает с ценой заказа.");
                _cashRegister.AcceptCardOrder();
                InputString = string.Empty;
                UpdateInputText();
            }
            else
            {
                Debug.Log("Сумма не совпадает с ценой заказа.");
                SoundPlayer.Instance.PlayError();
            }

            /*InputString = string.Empty;
            UpdateInputText();*/
        }

        public void ClearAll()
        {
            SoundPlayer.Instance.PlayButtonClick();
            InputString = string.Empty;
            UpdateInputText();
        }
        
        public void ClearLast()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            if (!string.IsNullOrEmpty(InputString))
            {
                InputString = InputString.Substring(0, InputString.Length - 1);
                UpdateInputText();
            }
        }
        
        private void UpdateInputText()
        {
            _inputText.text = InputString;
        }
    }
}