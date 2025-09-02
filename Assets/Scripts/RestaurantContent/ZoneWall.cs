using System;
using LoadingSceneContent;
using UI.Screens.ShopContent;
using UnityEngine;

namespace RestaurantContent
{
    public class ZoneWall : MonoBehaviour
    {
        [SerializeField] private ZoneUIProduct _zoneUIProduct;
        [SerializeField] private GameObject _closeDoor;
        [SerializeField] private GameObject _openDoor;
        [SerializeField] private GameObject _zoneEnvironment;
        [SerializeField] private bool _isDoor;
        [SerializeField] private LoadingGame _loadingGame;

        public event Action<bool> ActivityDoorChanged;

        private void OnEnable()
        {
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            _loadingGame.MirraSDKInitialization -= Init;
        }

        /*private void Start()
        {
            Activate();
        }*/

        private void Init()
        {
            Activate();
        }

        public void Activate()
        {
            if (_isDoor)
            {
                ActivityDoorChanged?.Invoke(_zoneUIProduct.IsBuyed());

                if (_closeDoor != null)
                    _closeDoor.SetActive(!_zoneUIProduct.IsBuyed());

                if (_openDoor != null)
                    _openDoor.SetActive(_zoneUIProduct.IsBuyed());

                _zoneEnvironment.SetActive(_zoneUIProduct.IsBuyed());
            }
            else
            {
                gameObject.SetActive(!_zoneUIProduct.IsBuyed());
            }
        }
    }
}