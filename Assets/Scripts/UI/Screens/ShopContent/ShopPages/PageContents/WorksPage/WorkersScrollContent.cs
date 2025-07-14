using SoContent;
using UI.Screens.ShopContent.WorkersContent;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.WorksPage
{
    public class WorkersScrollContent : PageScrollContent
    {
        [SerializeField] private WorkerUIProduct[] _workerUIProducts;
        [SerializeField] private WorkersConfig _workersConfig;

        public override void Init()
        {
            foreach (var workerUIProduct in _workerUIProducts)
            {
                WorkerConfig workerConfig = _workersConfig.GetWorkerConfig(workerUIProduct.WorkerType);
                
                if (workerConfig != null)
                    workerUIProduct.Init(workerConfig);
            }
        }
    }
}