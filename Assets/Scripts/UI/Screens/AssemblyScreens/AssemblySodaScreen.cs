using CameraContent;
using KitchenEquipmentContent;
using UnityEngine;

namespace UI.Screens.AssemblyScreens
{
    public class AssemblySodaScreen : AbstractScreen
    {
        [SerializeField] private GameObject _input;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private AssemblyDrinkTable _assemblyTable;
        [SerializeField] private GameObject[] _deactivateContent;
        
        public override void OpenScreen()
        {
            Debug.Log("OPENASSEMBLYSCREEN");
            base.OpenScreen();
            _input.SetActive(false);
            SetValue(false);
        }

        public override void CloseScreen()
        {
            _cameraPositionChanger.ReturnDefaultPosition();
            base.CloseScreen();
            _assemblyTable.SetValueCollider(true);

            if (Application.isMobilePlatform)
                _input.SetActive(true);
            
            SetValue(true);
        }
        
        private void SetValue(bool value)
        {
            foreach (var deactivateObject in _deactivateContent)
                deactivateObject.SetActive(value);
        }
    }
}