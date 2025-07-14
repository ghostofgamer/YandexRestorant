using Enums;
using KitchenEquipmentContent;
using UI.Buttons;
using UnityEngine;

public class SodaPourButton : AbstractButton
{
    [SerializeField] private SodaAssemblyTable _sodaAssemblyTable;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private int _index;

    public override void OnClick()
    {
        _sodaAssemblyTable.PourSoda(_itemType, _index);
    }
}