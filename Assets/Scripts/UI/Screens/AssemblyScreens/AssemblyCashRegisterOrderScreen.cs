using CameraContent;
using RestaurantContent.CashRegisterContent;
using UnityEngine;

namespace UI.Screens.AssemblyScreens
{
    public class AssemblyCashRegisterOrderScreen : AbstractScreen
    {
        [SerializeField] private GameObject _input;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private CashRegisterViewer _cashRegisterViewer;
        [SerializeField] private CashRegister _cashRegister;

        public override void OpenScreen()
        {
            Debug.Log("OPENASSEMBLYSCREEN");
            base.OpenScreen();
            _input.SetActive(false);
            _cashRegisterViewer.SetValuePanels(true);
        }

        public override void CloseScreen()
        {
            _cameraPositionChanger.ReturnDefaultPosition();
            base.CloseScreen();
            _input.SetActive(true);
            _cashRegisterViewer.SetValuePanels(false);
            _cashRegister.SetPlayerValue(false);
        }
    }
}