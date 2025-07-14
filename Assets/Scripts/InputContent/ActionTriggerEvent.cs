using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionTriggerEvent : MonoBehaviour, IPointerClickHandler
{
    private Action<PointerEventData> _clickAction;

    public void InitPointer(Action<PointerEventData> enterAction = null)
    {
        _clickAction = enterAction;
    }

    public void OnPointerClick(PointerEventData eventData) => _clickAction?.Invoke(eventData);
}