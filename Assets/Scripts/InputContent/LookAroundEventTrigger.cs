using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookAroundEventTrigger : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Action<PointerEventData> _downAction, _dragAction, _upAction, _enterAction, _exitAction;
    
    public void InitPointer(
        Action<PointerEventData> downAction, 
        Action<PointerEventData> dragAction = null, 
        Action<PointerEventData> upAction = null)
    {
        _downAction = downAction;
        _dragAction = dragAction;
        _upAction = upAction;
    }
    
    public void OnPointerDown(PointerEventData eventData) => _downAction?.Invoke(eventData);
    public void OnDrag(PointerEventData eventData) => _dragAction?.Invoke(eventData);
    public void OnPointerUp(PointerEventData eventData) => _upAction?.Invoke(eventData);
}