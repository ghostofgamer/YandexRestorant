using LoadingSceneContent;
using SaveContent;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class DeepFryerContainerSaver : MonoBehaviour
    {
        [SerializeField] private FryerContainer _fryerContainer;
        [SerializeField]private LoadingGame _loadingGame;

        /*private void Awake()
        {
            Load();
        }*/

        private void OnEnable()
        {
            _fryerContainer.ItemArrayValueChanged += Save;
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            _fryerContainer.ItemArrayValueChanged -= Save;
            _loadingGame.MirraSDKInitialization -= Init;
        }

        private void Init()
        {
            Load();
        }

        private void Load()
        {
            // int value = PlayerPrefs.GetInt("DeepFryerContainerValueWell" + _fryerContainer.ItemType, 0);
            int value = StorageHelper.GetInt("DeepFryerContainerValueWell" + _fryerContainer.ItemType, 0);
            _fryerContainer.ActivateItems(value);
        }

        private void Save(int wellValue)
        {
            // PlayerPrefs.SetInt("DeepFryerContainerValueWell" + _fryerContainer.ItemType, wellValue);
            StorageHelper.SetInt("DeepFryerContainerValueWell" + _fryerContainer.ItemType, wellValue);
        }
    }
}