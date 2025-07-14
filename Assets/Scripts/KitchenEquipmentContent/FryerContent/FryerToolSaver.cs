using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerToolSaver : MonoBehaviour
    {
        [SerializeField] private FryerTool _fryerTool;

        private void Awake()
        {
            Load();
        }

        private void OnEnable()
        {
            _fryerTool.ItemsValueChanged += Save;
        }

        private void OnDisable()
        {
            _fryerTool.ItemsValueChanged -= Save;
        }

        private void Load()
        {
            int rawValue = PlayerPrefs.GetInt("RawDeepFryerCount" + _fryerTool.ItemType, 0);
            int wellValue = PlayerPrefs.GetInt("WellDeepFryerCount" + _fryerTool.ItemType, 0);
            _fryerTool.Init(rawValue, wellValue);
        }

        private void Save(int rawCount, int wellCount)
        {
            PlayerPrefs.SetInt("RawDeepFryerCount" + _fryerTool.ItemType, rawCount);
            PlayerPrefs.SetInt("WellDeepFryerCount" + _fryerTool.ItemType, wellCount);
        }
    }
}