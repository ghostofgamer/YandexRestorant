using I2.Loc;
using TMPro;
using UnityEngine;

namespace DeliveryContent
{
    public class DeliveryViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _deliveryScreen;
        [SerializeField] private TMP_Text _timerDeliveryText;
        [SerializeField] private TMP_Text _amountDelivers;
        [SerializeField] private TMP_Text _amountEnergyPriceDelivers;
        [SerializeField] private Delivery _delivery;

        private void OnEnable()
        {
            _delivery.AmountItemsDeliveriesChanged += ShowAmountDeliveries;
            _delivery.TimeChanged += ShowTimer;
        }

        private void OnDisable()
        {
            _delivery.AmountItemsDeliveriesChanged -= ShowAmountDeliveries;
            _delivery.TimeChanged -= ShowTimer;
        }

        private void ShowAmountDeliveries(int amount)
        {
            _deliveryScreen.SetActive(amount > 0);
            _amountDelivers.text = amount.ToString();
            _amountEnergyPriceDelivers.text = amount.ToString();
        }

        private void ShowTimer(float currentTime)
        {
            int seconds = Mathf.CeilToInt(currentTime);
            _timerDeliveryText.text = $" {LocalizationManager.GetTermTranslation("Delivery in")}: {seconds / 60}:{seconds % 60:00}";
        }
    }
}