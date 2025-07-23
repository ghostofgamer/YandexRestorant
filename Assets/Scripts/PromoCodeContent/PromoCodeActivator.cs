using DeliveryContent;
using EnergyContent;
using Enums;
using PlayerContent.LevelContent;
using UnityEngine;
using WalletContent;

namespace PromoCodeContent
{
    public class PromoCodeActivator : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Delivery _delivery;
        [SerializeField] private Energy _energy;

        public void ActivatePrizePromo()
        {
            // AppMetrica.ReportEvent("ActivatePrizePromo");
            _wallet.Add(new DollarValue(50, 0));
            _delivery.SpawnPrize(ItemType.Bun, 2);
            _delivery.SpawnPrize(ItemType.RawCutlet, 2);
        }
    }
}