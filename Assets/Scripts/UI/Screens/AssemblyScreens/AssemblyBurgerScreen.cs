using CameraContent;
using UnityEngine;

namespace UI.Screens
{
    public class AssemblyBurgerScreen : AbstractScreen
    {
        [SerializeField] private GameObject _input;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private AssemblyTable _assemblyTable;
        [SerializeField] private GameObject[] _UIElements;

        public override void OpenScreen()
        {
            Debug.Log("OPENASSEMBLYSCREEN");
            base.OpenScreen();
            _input.SetActive(false);

            SetValueUIElement(false);
        }

        public override void CloseScreen()
        {
            _cameraPositionChanger.ReturnDefaultPosition();
            base.CloseScreen();
            _assemblyTable.SetValueCollider(true);

            if (Application.isMobilePlatform)
                _input.SetActive(true);

            SetValueUIElement(true);
        }

        private void SetValueUIElement(bool value)
        {
            foreach (var uiElement in _UIElements)
                uiElement.SetActive(value);
        }
    }
}