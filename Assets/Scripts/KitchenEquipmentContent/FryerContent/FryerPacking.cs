using AttentionHintContent;
using Enums;
using I2.Loc;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerPacking : MonoBehaviour
    {
        [SerializeField] private FryerTool[] _fryerTools;
        [SerializeField] private Transform[] _positions;

        public void Packing(ItemBasket itemBasket)
        {
            Debug.Log("itemBasket " + itemBasket);

            FryerTool compatibleTool = GetCompatibleFryerTool(itemBasket.ItemType);

            if (compatibleTool != null)
            {
                if (!compatibleTool.IsRaw)
                {
                    AttentionHintActivator.Instance.ShowHint(
                        LocalizationManager.GetTermTranslation("You can't put in raw ones: take away cooked ones"));
                    Debug.Log("лежат готовые фри");
                    return;
                }

                Debug.Log("Item type is compatible with fryer tool: " + compatibleTool.name);
                int emptyPosition = compatibleTool.GetCountInactiveItems();
                int activeItems = itemBasket.GetActiveValueItems();
                Debug.Log("emptyPosition " + emptyPosition + " activeItems " + activeItems);

                if (emptyPosition <= 0)
                    AttentionHintActivator.Instance.ShowHint(
                        LocalizationManager.GetTermTranslation("No place"));
                else if (activeItems <= 0)
                    AttentionHintActivator.Instance.ShowHint(
                        LocalizationManager.GetTermTranslation("The box is empty"));

                if (emptyPosition > 0 && activeItems > 0)
                {
                    int itemsToPlace = Mathf.Min(emptyPosition, activeItems);

                    Debug.Log("emptyPosition " + emptyPosition);
                    Debug.Log("activeItems " + activeItems);

                    compatibleTool.ActivateRawItems(itemsToPlace);
                    itemBasket.TransferProduct(itemsToPlace, compatibleTool.Positions);
                    // targetContainer.ActivateItems(itemsToPlace);
                }
            }
            else
            {
                Debug.Log("Item type is not compatible with any of the fryer tools.");
                AttentionHintActivator.Instance.ShowHint(
                    LocalizationManager.GetTermTranslation("Unsuitable product type"));
            }
        }

        public FryerTool GetCompatibleFryerTool(ItemType itemType)
        {
            foreach (var fryerTool in _fryerTools)
            {
                if (fryerTool.ItemType == itemType)
                {
                    return fryerTool;
                }
            }

            return null;
        }

        public int GetFullTools()
        {
            int value = 0;

            foreach (var fryerTool in _fryerTools)
            {
                if (fryerTool.GetCountActiveItems() > 0)
                    value++;
            }

            return value;
        }
    }
}