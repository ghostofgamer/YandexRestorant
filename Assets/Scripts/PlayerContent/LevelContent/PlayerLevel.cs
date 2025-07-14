using System;
using System.Collections.Generic;
using Io.AppMetrica;
using SettingsContent.SoundContent;
using UI;
using UnityEngine;

namespace PlayerContent.LevelContent
{
    public class PlayerLevel : MonoBehaviour
    {
        [SerializeField] private FlyValue _flyValue;
        
        public List<LevelConfig> levelConfigs;
        
        private int _minLevel = 1;
        private int _currentExp;
        private int _targetExp;

        public event Action<int> LevelChanged;
        public event Action<int, int> ExpChanged;
        
        public event Action<int> ExpAdded;
        public event Action LevelAdded;

        public int CurrentLevel { get; private set; }
        
        private void Start()
        {
            CurrentLevel = PlayerPrefs.GetInt("Level", _minLevel);
            _currentExp = PlayerPrefs.GetInt("Exp", 0);
            _targetExp = GetExpForLevel(CurrentLevel);
            LevelChanged?.Invoke(CurrentLevel);
            ExpChanged?.Invoke(_currentExp, _targetExp);
        }

        [ContextMenu("TestAddCurrentExp")]
        public void TestAddExp()
        {
            AddExp(15);
        }

        public void AddExp(int valueExp)
        {
            if (valueExp <= 0)
                return;
            
            _flyValue.ShowFly(valueExp);
            _currentExp += valueExp;
            
            ExpAdded?.Invoke(valueExp);
            
            while (_currentExp >= _targetExp && CurrentLevel < levelConfigs.Count)
            {
                int excessExp = _currentExp - _targetExp;
                LevelUp();
                _currentExp = excessExp;
            }
            
            PlayerPrefs.SetInt("Exp", _currentExp);
            ExpChanged?.Invoke(_currentExp, _targetExp);
        }

        private void LevelUp()
        {
           SoundPlayer.Instance.PlayLevelUp();
            CurrentLevel++;
            AppMetrica.ReportEvent("LevelUp", "{\"" + CurrentLevel.ToString() + "\":null}");
            
            PlayerPrefs.SetInt("Level", CurrentLevel);
            _targetExp = GetExpForLevel(CurrentLevel);
            Debug.Log("_targetExp " + _targetExp);
            LevelAdded?.Invoke();
            LevelChanged?.Invoke(CurrentLevel);
            ExpChanged?.Invoke(_currentExp, _targetExp);
        }
        
        private int GetExpForLevel(int level)
        {
            foreach (var config in levelConfigs)
            {
                if (config.level == level)
                {
                    return config.expRequired;
                }
            }
            return int.MaxValue;
        }
    }
}