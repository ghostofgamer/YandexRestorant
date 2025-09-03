using LoadingSceneContent;
using SaveContent;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerToolSaver : MonoBehaviour
    {
        [SerializeField] private FryerTool _fryerTool;
        [SerializeField]private LoadingGame _loadingGame;

        /*private void Awake()
        {
            Load();
        }*/

        private void OnEnable()
        {
            _fryerTool.ItemsValueChanged += Save;
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            _fryerTool.ItemsValueChanged -= Save;
            _loadingGame.MirraSDKInitialization -= Init;
        }

        private void Init()
        {
            Load();
        }

        private void Load()
        {
            // int rawValue = PlayerPrefs.GetInt("RawDeepFryerCount" + _fryerTool.ItemType, 0);
            int rawValue = StorageHelper.GetInt("RawDeepFryerCount" + _fryerTool.ItemType, 0);
            // int wellValue = PlayerPrefs.GetInt("WellDeepFryerCount" + _fryerTool.ItemType, 0);
            int wellValue = StorageHelper.GetInt("WellDeepFryerCount" + _fryerTool.ItemType, 0);
            _fryerTool.Init(rawValue, wellValue);
        }

        private void Save(int rawCount, int wellCount)
        {
            // PlayerPrefs.SetInt("RawDeepFryerCount" + _fryerTool.ItemType, rawCount);
            StorageHelper.SetInt("RawDeepFryerCount" + _fryerTool.ItemType, rawCount);
            // PlayerPrefs.SetInt("WellDeepFryerCount" + _fryerTool.ItemType, wellCount);
            StorageHelper.SetInt("WellDeepFryerCount" + _fryerTool.ItemType, wellCount);
        }
    }
}