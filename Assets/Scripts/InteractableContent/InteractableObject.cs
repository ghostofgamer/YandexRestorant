using System;
using Interfaces;
using PlayerContent;
using UnityEngine;

namespace InteractableContent
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private Outline[] _outlines;
        // [SerializeField] private HighlightEffect[] _highlightEffects;

        public event Action<PlayerInteraction> OnAction;

        public void EnableOutline()
        {
            // _outline.enabled = true;
            // if (_highlightEffects.Length > 0)
            //     foreach (var effect in _highlightEffects)
            //         effect.enabled = true;

            if (_outlines.Length > 0)
                foreach (var outline in _outlines)
                    outline.OutlineWidth = 10;
        }

        public void DisableOutline()
        {
            // _outline.enabled = false;
            /*if (_outline != null)
                _outline.OutlineWidth = 0;*/
            
            if (_outlines.Length > 0)
                foreach (var outline in _outlines)
                    outline.OutlineWidth = 0;

            // if (_highlightEffects.Length > 0)
            //     foreach (var effect in _highlightEffects)
            //         effect.enabled = false;
        }

        public virtual void Action(PlayerInteraction playerInteraction)
        {
            Debug.Log("Interactable to mey " + gameObject.name);
            OnAction?.Invoke(playerInteraction);
        }
    }
}