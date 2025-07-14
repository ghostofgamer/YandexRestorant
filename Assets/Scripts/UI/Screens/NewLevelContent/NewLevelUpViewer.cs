using System.Collections;
using System.Linq;
using I2.Loc;
using PlayerContent.LevelContent;
using SoContent;
using TMPro;
using UnityEngine;

namespace UI.Screens.NewLevelContent
{
    public class NewLevelUpViewer : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private TMP_Text _levelStarText;
        [SerializeField] private TMP_Text _leverRestText;
        [SerializeField] private TMP_Text _newProductText;
        [SerializeField] private TMP_Text _newRecipesText;
        [SerializeField] private TMP_Text _newMAchineText;
        [SerializeField] private RewardsLevelingUpConfig _rewardsLevelingUpConfig;
        [SerializeField] private GameObject _completeButton;
        [SerializeField] private GameObject _newProductObject;
        [SerializeField] private GameObject _newRecipesObject;
        [SerializeField] private GameObject _newMachineObject;
        [SerializeField] private NewLevelUpScreen _newLevelUpScreen;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);
        private WaitForSeconds _waitForHalfSeconds = new WaitForSeconds(0.6f);
        private string _productsList;
        private string _recipesList;
        private string _machineList;

        public void Init()
        {
            _levelStarText.text = _playerLevel.CurrentLevel.ToString();
            _leverRestText.text = $"{LocalizationManager.GetTermTranslation("Level")}-{_playerLevel.CurrentLevel}";

            RewardLeveling rewardLeveling = _rewardsLevelingUpConfig.GetLevelData(_playerLevel.CurrentLevel);

            _productsList = string.Join(", ",
                rewardLeveling.products.Select(product => LocalizationManager.GetTermTranslation(product.ToString())));

            _recipesList = string.Join(", ",
                rewardLeveling.recipes.Select(product => LocalizationManager.GetTermTranslation(product.ToString())));

            _machineList = string.Join(", ",
                rewardLeveling.equipment.Select(product => LocalizationManager.GetTermTranslation(product.ToString())));

            if (rewardLeveling != null)
            {
                _newProductText.text = $"{LocalizationManager.GetTermTranslation("NewProduct")} {_productsList}";
                _newRecipesText.text = $"{LocalizationManager.GetTermTranslation("NewRecipes")} {_recipesList}";
                _newMAchineText.text = $"{LocalizationManager.GetTermTranslation("NewEquipment")} {_machineList}";
            }
        }

        public void ShowRewardLeveling()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartShowInfo());
        }

        private IEnumerator StartShowInfo()
        {
            SetValue(false);
            
            if (AreAllListsEmpty())
                _newLevelUpScreen.CloseScreen();

            yield return _waitForSeconds;

            if (!string.IsNullOrEmpty(_productsList))
            {
                _newProductObject.SetActive(true);
                yield return _waitForHalfSeconds;
            }

            if (!string.IsNullOrEmpty(_recipesList))
            {
                _newRecipesObject.SetActive(true);
                yield return _waitForHalfSeconds;
            }

            if (!string.IsNullOrEmpty(_machineList))
            {
                _newMachineObject.SetActive(true);
                yield return _waitForHalfSeconds;
            }

            _completeButton.SetActive(true);
        }

        private void SetValue(bool value)
        {
            _completeButton.SetActive(value);
            _newProductObject.SetActive(value);
            _newRecipesObject.SetActive(value);
            _newMachineObject.SetActive(value);
        }

        public bool AreAllListsEmpty()
        {
            return string.IsNullOrEmpty(_productsList) &&
                   string.IsNullOrEmpty(_recipesList) &&
                   string.IsNullOrEmpty(_machineList);
        }
    }
}