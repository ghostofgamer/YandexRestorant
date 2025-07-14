using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrdersContent.OrderPromptContent
{
    public class OrdersPromptsCounter : MonoBehaviour
    {
        [SerializeField] private OrderPrompt[] _orderPrompts;
        [SerializeField] private OrdersCounter _ordersCounter;
        [SerializeField] private Button _toggleButton;
        [SerializeField] private TMP_Text _amountOrdersValueText;
        [SerializeField] private GameObject _amountOrdersContatainer;
        [SerializeField] private TMP_Text _arrowText;

        private bool _isExpanded = false;

        private void OnEnable()
        {
            _ordersCounter.OrdersChanged += UpdateOrdersPrompts;
            _toggleButton.onClick.AddListener(OnToggleButtonClicked);
        }

        private void OnDisable()
        {
            _ordersCounter.OrdersChanged -= UpdateOrdersPrompts;
            _toggleButton.onClick.RemoveListener(OnToggleButtonClicked);
        }

        private void UpdateOrdersPrompts(List<Order> orders)
        {
            foreach (var orderPrompt in _orderPrompts)
                orderPrompt.Deactivate();

            if (orders.Count <= 1)
            {
                _orderPrompts[0].InitOrder(orders[0]);
                _orderPrompts[0].Activate();
                _toggleButton.gameObject.SetActive(false);
                _amountOrdersValueText.text = _ordersCounter.CurrentOrders.Count.ToString();
            }
            else
            {
                _toggleButton.gameObject.SetActive(true);

                if (_isExpanded)
                {
                    ShowAmountOrders(false);
                        
                    for (int i = 0; i < orders.Count; i++)
                    {
                        _orderPrompts[i].InitOrder(orders[i]);
                        _orderPrompts[i].Activate();
                    }
                }
                else
                {
                    ShowAmountOrders(true);
                    _orderPrompts[0].InitOrder(orders[0]);
                    _orderPrompts[0].Activate();
                }
            }

            /*for (int i = 0; i < orders.Count; i++)
            {
                _orderPrompts[i].InitOrder(orders[i]);
                _orderPrompts[i].Activate();
            }*/
        }

        private void OnToggleButtonClicked()
        {
            _isExpanded = !_isExpanded;
            UpdateOrdersPrompts(_ordersCounter.CurrentOrders);
        }

        private void ShowAmountOrders(bool value)
        {
            _arrowText.text = value ? ">" : "<";
            _amountOrdersContatainer.SetActive(value);
            _amountOrdersValueText.text = _ordersCounter.CurrentOrders.Count.ToString();
        }
    }
}