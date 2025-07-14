using System;
using PlayerContent.LevelContent;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class ZonesScrollContent : PageScrollContent
    {
        [SerializeField] private ZoneUIProduct[] _zoneUIProducts;
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
            foreach (var zoneUIProduct in _zoneUIProducts)
                zoneUIProduct.Init(level);
        }
    }
}
