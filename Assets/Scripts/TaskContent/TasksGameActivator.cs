using System;
using PlayerContent.LevelContent;
using UnityEngine;

namespace TaskContent
{
    public class TasksGameActivator : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private FortuneTask fortuneTask;

        private void OnEnable()
        {
            _playerLevel.LevelChanged += ActivateTask;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= ActivateTask;
        }

        private void Start()
        {
            ActivateTask(_playerLevel.CurrentLevel);
        }

        private void ActivateTask(int levelPlayer)
        {
            if (levelPlayer >= 3)
                StartFortuneTask();
        }

        private void StartFortuneTask()
        {
            if (PlayerPrefs.GetInt("FreeSpinUsed", 0) > 0)
                return;
            
            fortuneTask.ActivateTask();
        }
    }
}