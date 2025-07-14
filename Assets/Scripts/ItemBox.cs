using System;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private GameObject _openBox;
    [SerializeField] private GameObject _closeBox;
    [SerializeField] private Draggable _draggable;

    public bool IsOpen { get; private set; }

    private void OnEnable()
    {
        _draggable.DraggablePicked += OpenBox;
        _draggable.DraggableThrowed += CloseBox;
        _draggable.PutOnShelfCompleting += CloseBox;
    }

    private void OnDisable()
    {
        _draggable.DraggablePicked -= OpenBox;
        _draggable.DraggableThrowed += CloseBox;
        _draggable.PutOnShelfCompleting -= CloseBox;
    }

    public void SetValue(bool value)
    {
        IsOpen = value;
        _openBox.SetActive(value);
        _closeBox.SetActive(!value);
    }

    private void OpenBox()
    {
        SetValue(true);
    }

    private void CloseBox()
    {
        SetValue(false);
    }
}