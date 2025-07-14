using System;
using Enums;
using ShelfContent;
using UnityEngine;
using UnityEngine.UI;

namespace ItemContent
{
    public class ItemDrinkPackage : MonoBehaviour
    {
        [SerializeField] private Draggable _draggable;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Image _imageFullness;
        [SerializeField] private GameObject _canvasFullness;

        private int _maxFullnes = 100;
        
        public Shelf Shelf { get; private set; }
        
        public int CurrentFullness { get; private set; }

        public ItemType ItemType => _itemType;

        private bool _firstFullness=true;

        private void OnEnable()
        {
            _draggable.DraggablePicked += () => ActivateCanvas();
            _draggable.DraggableThrowed += () => _canvasFullness.gameObject.SetActive(false);
            _draggable.PutOnShelfCompleting += () => _canvasFullness.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _draggable.DraggablePicked -= ()  => ActivateCanvas();
            _draggable.DraggableThrowed -= () => _canvasFullness.gameObject.SetActive(false);
            _draggable.PutOnShelfCompleting -= () => _canvasFullness.gameObject.SetActive(false);
        }

        private void Start()
        {
            if (!_firstFullness)
                return;
            
            CurrentFullness = _maxFullnes;
            UpdateFullUI();
        }

        public void PourOut(int value)
        {
            CurrentFullness -= value;

            if (CurrentFullness <= 0)
                CurrentFullness = 0;

            UpdateFullUI();
        }

        private void UpdateFullUI()
        {
            _imageFullness.fillAmount = (float)CurrentFullness / _maxFullnes;
        }
        
        public void SetShelf(Shelf shelf)
        {
            Shelf = shelf;
        }

        public void SetFullness(int fullness)
        {
            Debug.Log("SETFULLNES");
            _firstFullness = false;
            CurrentFullness = fullness;
            UpdateFullUI();
        }

        private void ActivateCanvas()
        {
            if (Shelf != null)
            {
                Shelf.RemoveDrinkPackage(this);
                Shelf = null;
            }

            _canvasFullness.gameObject.SetActive(true);
        }
    }
}