using DeliveryContent;
using EnergyContent;
using Enums;
using FortuneContent;
using UnityEngine;
using WalletContent;

namespace DailyRewardContent
{
    public class Prize : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Delivery _delivery;
        [SerializeField] private Fortune _fortune;
        [SerializeField] private Energy _energy;

        public void Claim(int index)
        {
            switch (index)
            {
                case 0:
                    _wallet.Add(new DollarValue(25, 0));
                    break;

                case 1:
                    _delivery.SpawnPrize(ItemType.Bun, 3);
                    break;

                case 2:
                    _wallet.Add(new DollarValue(75, 0));
                    break;

                case 3:
                    _delivery.SpawnPrize(ItemType.RawCutlet, 5);
                    break;

                case 4:
                    _wallet.Add(new DollarValue(150, 0));
                    break;

                case 5:
                    _energy.IncreaseEnergy(25);
                    break;

                case 6:
                    TakeSuperPrize();
                    break;
            }
        }

        private void TakeSuperPrize()
        {
            _wallet.Add(new DollarValue(300, 0));
            _delivery.SpawnPrize(ItemType.RawCutlet, 3);
            _delivery.SpawnPrize(ItemType.Bun, 3);
                
            /*if (_decorationSystem.GetActivationValueDecoration(_decorationSystem.CurrentDailyRewardDecoration))
            {
                _decorationSystem.ActivateDecoration(_decorationSystem.CurrentDailyRewardDecoration);
                Debug.Log("актвируем ДЕКОР ");
            }
            else
            {
                _currencyController.AddCurrencyFastMoney(CurrencyType.Soft, new(500, 0), true);
                Debug.Log("актвируем БАБКИ ");
            }*/
        }
    }
}