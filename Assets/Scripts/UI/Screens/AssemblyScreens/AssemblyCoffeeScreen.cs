using CameraContent;
using KitchenEquipmentContent;
using UI.Screens;
using UnityEngine;

public class AssemblyCoffeeScreen : AbstractScreen
{
    [SerializeField] private GameObject _input;
    [SerializeField] private CameraPositionChanger _cameraPositionChanger;
    [SerializeField] private AssemblyDrinkTable _assemblyTable;
        
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
