using Enums;
using Io.AppMetrica;
using TMPro;
using UI.Screens.TutorialScreens;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialContent
{
    public class NameRestaurant : MonoBehaviour
    {
        [SerializeField] private TutorialType _tutorialType;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private NameRestaurantScreen _nameRestaurantScreen;
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private Button _saveButton;
        [SerializeField] private TMP_Text _nameText;

        private void Start()
        {
            _nameText.text = PlayerPrefs.GetString("RestaurantName", "Restaurant");
            _saveButton.interactable = false;
        }

        public void OnNameChanged(string name)
        {
            // Проверяем, что длина имени находится в пределах от 1 до 13 символов
            _saveButton.interactable = name.Length >= 1 && name.Length <= 13;
        }

        public void Save()
        {
            string restaurantName = _nameInputField.text;

            if (!string.IsNullOrEmpty(restaurantName) && restaurantName.Length <= 13)
            {
                if ((int)_tutorial.CurrentType == (int)_tutorialType)
                {
                    _tutorial.SetCurrentTutorialStage(_tutorialType);
                }

                _nameRestaurantScreen.CloseScreen();
                _nameText.text = restaurantName;
                PlayerPrefs.SetString("RestaurantName", restaurantName);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogError("Invalid restaurant name length. Name must be between 1 and 13 characters.");
            }
        }
    }
}