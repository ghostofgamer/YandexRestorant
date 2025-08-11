using LoadingSceneContent;
using UI.Buttons;
using UnityEngine;

public class PurchasePriceInitializer : MonoBehaviour
{
    [SerializeField]private LoadingGame _loadingGame;
    [SerializeField] private PurchaseInapButon[] _purchaseInapButon;
    
    protected void OnEnable()
    {
        _loadingGame.MirraSDKInitialization += GetProductPrice;
    }

    protected void OnDisable()
    {
  
        _loadingGame.MirraSDKInitialization -= GetProductPrice;
    }
    
    public void GetProductPrice()
    {
        foreach (var purchaseInapButon in _purchaseInapButon)
            purchaseInapButon.GetProductPrice();
    }
}