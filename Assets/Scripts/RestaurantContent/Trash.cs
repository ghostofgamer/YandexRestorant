using AttentionHintContent;
using Enums;
using I2.Loc;
using InteractableContent;
using ItemContent;
using PlayerContent;
using TutorialContent;
using UnityEngine;

namespace RestaurantContent
{
    public class Trash : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private BoxesCounter _boxesCounter;

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
            if (playerInteraction.CurrentDraggable != null)
            {
                if (!GetCheckEmpty(playerInteraction.CurrentDraggable.gameObject))
                {
                    AttentionHintActivator.Instance.ShowHint(
                        LocalizationManager.GetTermTranslation(
                            "You can't throw away a box if there are products in it"));
                    Debug.Log("Нельзя это выкинуть ");
                    return;
                }

                playerInteraction.ThrowItem();
            }
            else
            {
                AttentionHintActivator.Instance.ShowHint(
                    LocalizationManager.GetTermTranslation("Throw away nothing"));
                Debug.Log("Не то в руках или вообще пусто ");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Draggable draggable = other.GetComponentInParent<Draggable>();

            if (draggable == null)
            {
                Debug.Log("Draggable component not found on the collided object.");
                return;
            }

            if (!GetCheckEmpty(draggable.gameObject))
            {
                Debug.Log("Нельзя это выкинуть ");
                // AttentionHintActivator.Instance.ShowHint("Нельзя выкинуть коробку, если там продукты");
                return;
            }

            if (!draggable.InHands)
            {
                _boxesCounter.RemoveBox(draggable.gameObject);
                draggable.gameObject.SetActive(false);

                if (_tutorial.CurrentType == TutorialType.ThrowEmptyBoxInTrash)
                    _tutorial.SetCurrentTutorialStage(TutorialType.ThrowEmptyBoxInTrash);

                if (_tutorial.CurrentType == TutorialType.ThrowEmptyBoxInTrashSecond)
                    _tutorial.SetCurrentTutorialStage(TutorialType.ThrowEmptyBoxInTrashSecond);

                if (_tutorial.CurrentType == TutorialType.ThrowEmptyBoxInTrashThird)
                    _tutorial.SetCurrentTutorialStage(TutorialType.ThrowEmptyBoxInTrashThird);
            }
        }

        private bool GetCheckEmpty(GameObject draggable)
        {
            if (draggable.TryGetComponent(out ItemBasket itemBasket))
            {
                if (itemBasket.IsAdditionalItemsBasket)
                {
                    int[] value = itemBasket.GetActiveValueArrayItems();

                    foreach (var t in value)
                    {
                        if (t > 0)
                        {
                            Debug.Log("не пустая Additional коробка");
                            return false;
                        }
                    }
                }
                else
                {
                    if (itemBasket.GetActiveValueItems() > 0)
                    {
                        Debug.Log("не пустая коробка");
                        return false;
                    }
                }
            }

            if (draggable.TryGetComponent(out ItemDrinkPackage itemDrinkPackage))
            {
                if (itemDrinkPackage.CurrentFullness > 0)
                {
                    Debug.Log("не пустая коробка c напитками");
                    return false;
                }
            }

            return true;
        }
    }
}