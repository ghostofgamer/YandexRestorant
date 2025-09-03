using InteractableContent;
using LoadingSceneContent;
using PlayerContent;
using UI.Screens;
using UI.Screens.EquipmentContent;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class Fryer : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private GameObject[] _friersContentTables;
        [SerializeField] private EquipmentUIProduct _equipmentUIProduct;
        [SerializeField] private FryerPacking _fryerPacking;
        [SerializeField] private FryerFrying _fryerFrying;
        [SerializeField]private LoadingGame _loadingGame;
        
        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
            _loadingGame.MirraSDKInitialization -= Init;
        }
        
        /*private void Start()
        {
            foreach (var friersContentTable in _friersContentTables)
                friersContentTable.SetActive(_equipmentUIProduct.IsBuyed());
        }*/

        private void Init()
        {
            foreach (var friersContentTable in _friersContentTables)
                friersContentTable.SetActive(_equipmentUIProduct.IsBuyed());
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.CurrentDraggable != null)
                _fryerPacking.Packing(playerInteraction.CurrentDraggable.GetComponent<ItemBasket>());
            else
                _fryerFrying.Fry();
        }
    }
}