using CameraContent;
using RestaurantContent.CashRegisterContent;
using UnityEngine;

namespace UI.Screens.AssemblyScreens
{
    public class AssemblyCardRegisterOrderScreen : AbstractScreen
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
            _cashRegisterViewer.SetCardPaymentSetValuePanels(true);
        }

        public override void CloseScreen()
        {
            _cameraPositionChanger.ReturnDefaultPosition();
            base.CloseScreen();

            if (Application.isMobilePlatform)
                _input.SetActive(true);

            _cashRegisterViewer.SetCardPaymentSetValuePanels(false);
            _cashRegister.SetPlayerValue(false);
        }
    }
}