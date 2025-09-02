using System;
using LoadingSceneContent;
using PlayerContent.LevelContent;
using SaveContent;
using UnityEngine;

namespace TaskContent
{
    public class TasksGameActivator : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private FortuneTask fortuneTask;
        [SerializeField]private LoadingGame _loadingGame;

        private void OnEnable()
        {
            _playerLevel.LevelChanged += ActivateTask;
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= ActivateTask;
            _loadingGame.MirraSDKInitialization -= Init;
        }

        /*private void Start()
        {
            ActivateTask(_playerLevel.CurrentLevel);
        }*/

        private void Init()
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
            /*if (PlayerPrefs.GetInt("FreeSpinUsed", 0) > 0)
                return;*/
            
            if (StorageHelper.GetInt("FreeSpinUsed", 0) > 0)
                return;
            
            fortuneTask.ActivateTask();
        }
    }
}