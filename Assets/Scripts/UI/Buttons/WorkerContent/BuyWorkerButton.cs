using SettingsContent.SoundContent;
using UI.Buttons;
using UI.Screens.ShopContent.WorkersContent;
using UnityEngine;

public class BuyWorkerButton :AbstractButton
{
    [SerializeField] private WorkerUIProduct _workerUIProduct;
    
    public override void OnClick()
    {
        SoundPlayer.Instance.PlayButtonClick();
        _workerUIProduct.BuyWorker();
    }
}