using TMPro;
using UnityEngine;
using WalletContent;

namespace UI
{
    public class FlyValue : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private Color _color;

        public void ShowFly(DollarValue dollarValue, bool profitValue)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            _text.color = profitValue ? Color.green : Color.red;
            _text.text = dollarValue.ToString();
        }

        public void ShowFly(int value)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            
            _text.color = value >= 0 ? Color.green : Color.red;
            _text.text = value >= 0 ? $"+{value.ToString()}" : $"{value.ToString()}";
        }
    }
}