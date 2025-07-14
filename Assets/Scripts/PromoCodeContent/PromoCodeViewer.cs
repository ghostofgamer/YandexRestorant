using Enums;
using I2.Loc;
using TMPro;
using UI.Screens;
using UnityEngine;

namespace PromoCodeContent
{
    public class PromoCodeViewer : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _promoCodeInputField;
        [SerializeField] private TMP_Text _messageFailedText;
        [SerializeField] private TMP_Text _messageWellDoneText;
        [SerializeField] private GameObject _failTextBackground;
        [SerializeField] private GameObject _WellDoneTextBackground;
        [SerializeField] private PromoCodeScreen _promoCodeScreen;
        [SerializeField] private PromoCodeActivator _promoCodeActivator;

        public void AcceptPromoCode()
        {
            string enteredCode = _promoCodeInputField.text.Trim().ToUpper();
            PromoCodesType currentPromoCode = PromoCodesType.Cashier2025;

            string activePromoCode = currentPromoCode.ToString().ToUpper();

            if (enteredCode == activePromoCode)
            {
                if (PlayerPrefs.GetInt("AcceptedCode" + currentPromoCode, 0) == 1)
                {
                    Debug.Log("Этот промо-код уже был куплен.");
                    // _messageFailedText.text = "This promo code has already been purchased.";
                    _messageFailedText.text =
                        LocalizationManager.GetTermTranslation("This promo code has already been purchased.");
                    _failTextBackground.SetActive(true);
                }
                else
                {
                    _promoCodeActivator.ActivatePrizePromo();
                    PlayerPrefs.SetInt("AcceptedCode" + currentPromoCode, 1);
                    PlayerPrefs.Save();
                    _promoCodeInputField.text = "";
                    // _messageWellDoneText.text= "Right! Get prizes in the delivery area!";
                    _messageWellDoneText.text= LocalizationManager.GetTermTranslation("Right! Get prizes in the delivery area!");
                    _WellDoneTextBackground.SetActive(true);
                    // _promoCodeScreen.CloseScreen();
                }
            }
            else
            {
                Debug.Log("Неверный промо-код.");
                // _messageFailedText.text = "Invalid promo code.";
                _messageFailedText.text = LocalizationManager.GetTermTranslation("Invalid promo code.");
                _failTextBackground.SetActive(true);
            }
            
            _promoCodeInputField.text = "";
        }
    }
}