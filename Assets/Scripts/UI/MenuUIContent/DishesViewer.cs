using I2.Loc;
using SettingsContent;
using TMPro;
using UnityEngine;
using WalletContent;

namespace UI.MenuUIContent
{
    public class DishesViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _requiredText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private DishesUIItem _dishesUIItem;
        [SerializeField] private LanguageChanger _languageChanger;

        private DollarValue _valueProfit;
        private DollarValue _valuePrice;
        private DollarValue _valueCost;

        private void OnEnable()
        {
            _dishesUIItem.ChangeCurrentPrice += ShowCurrentPrice;
            _dishesUIItem.ChangeProfitPrice += ShowProfit;
            _dishesUIItem.InitCompleted += InitBaseInfo;
            _languageChanger.LanguageChanged += ChangeLocalization;
        }

        private void OnDisable()
        {
            _dishesUIItem.ChangeCurrentPrice -= ShowCurrentPrice;
            _dishesUIItem.ChangeProfitPrice -= ShowProfit;
            _dishesUIItem.InitCompleted -= InitBaseInfo;
            _languageChanger.LanguageChanged -= ChangeLocalization;
        }

        private void ShowProfit(DollarValue valueProfit)
        {
            _valueProfit = valueProfit;
            _profitText.text = $"{LocalizationManager.GetTermTranslation("Profit:")}{valueProfit}";
        }

        private void ShowCurrentPrice(DollarValue valueProfit, Color color)
        {
            _valuePrice = valueProfit;
            _priceText.text = $"{LocalizationManager.GetTermTranslation("Price")}:{valueProfit}";
            _priceText.color = color;
        }

        private void InitBaseInfo(string requiredInfo, DollarValue costValue)
        {
            _valueCost = costValue;
            _requiredText.text = requiredInfo;
            _costText.text = $"{LocalizationManager.GetTermTranslation("Cost")}:{costValue}";
        }

        private void ChangeLocalization()
        {
            _profitText.text = $"{LocalizationManager.GetTermTranslation("Profit:")}{_valueProfit}";
            _priceText.text = $"{LocalizationManager.GetTermTranslation("Price")}:{_valuePrice}";
            _costText.text = $"{LocalizationManager.GetTermTranslation("Cost")}:{_valueCost}";
        }
    }
}