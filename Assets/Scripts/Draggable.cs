using System;
using DG.Tweening;
using InteractableContent;
using Interfaces;
using PlayerContent;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Draggable : MonoBehaviour, IDraggable
{
    [SerializeField] private Transform _parentObject;
    [SerializeField] private InteractableObject _interactableObject;

    public event Action DraggablePicked;

    public event Action DraggableThrowed;

    public event Action PutOnShelfCompleting;

    public bool InHands;

    private void OnEnable()
    {
        _interactableObject.OnAction += Drag;
    }

    private void OnDisable()
    {
        _interactableObject.OnAction -= Drag;
    }

    public virtual void Drag(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.CurrentDraggable != null || playerInteraction.PlayerTray.IsActive)
        {
            Debug.Log("Return");
        }
        else
        {
            InHands = true;
            Debug.Log("DRAG");
            _parentObject.transform.parent = playerInteraction.DraggablePosition;
            _parentObject.DOLocalMove(Vector3.zero, 0.15f).SetEase(Ease.InOutQuad);
            _parentObject.DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.InOutQuad);

            playerInteraction.SetDraggableObject(this);
            DraggablePicked?.Invoke();
        }
    }

    public void Throw()
    {
        InHands = false;
        DraggableThrowed?.Invoke();
    }

    public void PutOnShelf()
    {
        InHands = false;
        PutOnShelfCompleting?.Invoke();
    }
}