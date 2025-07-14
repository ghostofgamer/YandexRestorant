using SettingsContent.SoundContent;
using UI.Screens.ShopContent.WorkersContent;
using UnityEngine;
using WalletContent;

namespace WorkerContent.Upgrade
{
    public class WorkerUpgrade : MonoBehaviour
    {
        [SerializeField] private Worker _worker;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private WorkerUIProduct _workerUIProduct;

        public void Upgrade()
        {
            if (_wallet.DollarValue.ToTotalCents() < _workerUIProduct.CurrentConfig.PriceUpgrade.ToTotalCents())
            {
                Debug.Log("Денег не хватает");
                return;
            }
            
            SoundPlayer.Instance.PlayPayment();
            _wallet.Subtract(_workerUIProduct.CurrentConfig.PriceUpgrade);
            _worker.NextLevel();
            _workerUIProduct.InitUpgradeInfo();
        }
    }
}