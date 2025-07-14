using InteractableContent;
using PlayerContent;
using RestaurantContent.TableContent;
using UnityEngine;

namespace GarbageContent
{
    public class GarbagePackage : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private TableCleanliness _tableCleanliness;

        public bool IsActive { get; private set; }

        private void OnEnable()
        {
            _interactableObject.OnAction += Action;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= Action;
        }

        private void Action(PlayerInteraction playerInteraction)
        {
            Clean();
        }

        public void SetValue(bool isActive)
        {
            IsActive = isActive;
            gameObject.SetActive(isActive);
        }

        public void Clean()
        {
            SetValue(false);
            // gameObject.SetActive(false);
            _tableCleanliness.DecreasePollutionLevel();
        }
    }
}