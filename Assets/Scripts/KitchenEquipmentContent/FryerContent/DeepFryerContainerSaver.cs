using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class DeepFryerContainerSaver : MonoBehaviour
    {
        [SerializeField] private FryerContainer _fryerContainer;

        private void Awake()
        {
            Load();
        }

        private void OnEnable()
        {
            _fryerContainer.ItemArrayValueChanged += Save;
        }

        private void OnDisable()
        {
            _fryerContainer.ItemArrayValueChanged -= Save;
        }

        private void Load()
        {
            int value = PlayerPrefs.GetInt("DeepFryerContainerValueWell" + _fryerContainer.ItemType, 0);
            _fryerContainer.ActivateItems(value);
        }

        private void Save(int wellValue)
        {
            PlayerPrefs.SetInt("DeepFryerContainerValueWell" + _fryerContainer.ItemType, wellValue);
        }
    }
}