using System.Collections.Generic;
using PlayerContent.LevelContent;
using UnityEngine;

namespace NewMenuContent
{
    public class NewMenuButtonActivator : MonoBehaviour
    {
        [SerializeField] private GameObject _newMenuButton;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private List<int> _levelsToActivateButton = new List<int>();

        private void OnEnable()
        {
            _playerLevel.LevelAdded += CheckOpenNewMenu;
        }

        private void OnDisable()
        {
            _playerLevel.LevelAdded -= CheckOpenNewMenu;
        }

        private void CheckOpenNewMenu()
        {
            _newMenuButton.SetActive(_levelsToActivateButton.Contains(_playerLevel.CurrentLevel));
        }
    }
}