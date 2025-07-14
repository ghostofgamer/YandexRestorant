using CameraContent;
using UnityEngine;

namespace UI.Screens
{
    public class AssemblyBurgerScreen : AbstractScreen
    {
        [SerializeField] private GameObject _input;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private AssemblyTable _assemblyTable;
        
        public override void OpenScreen()
        {
            Debug.Log("OPENASSEMBLYSCREEN");
            base.OpenScreen();
            _input.SetActive(false);
        }

        public override void CloseScreen()
        {
            _cameraPositionChanger.ReturnDefaultPosition();
            base.CloseScreen();
            _assemblyTable.SetValueCollider(true);
            _input.SetActive(true);
        }
    }
}