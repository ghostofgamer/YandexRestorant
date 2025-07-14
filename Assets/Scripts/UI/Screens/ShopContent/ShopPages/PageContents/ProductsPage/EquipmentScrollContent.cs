using PlayerContent.LevelContent;
using UI.Screens.EquipmentContent;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class EquipmentScrollContent : PageScrollContent
    {
        [SerializeField] private EquipmentUIProduct[] _equipmentUIProducts;
        [SerializeField] private PlayerLevel _playerLevel;
        
        private void OnEnable()
        {
            _playerLevel.LevelChanged += Initialization;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= Initialization;
        }
        
        public override void Init()
        {
            Initialization(_playerLevel.CurrentLevel);
            Debug.Log("GasmeObj " + gameObject.name);
        }
        
        private void Initialization(int level)
        {
            foreach (var equipmentUIProduct in _equipmentUIProducts)
                equipmentUIProduct.Init(level);
        }
    }
}