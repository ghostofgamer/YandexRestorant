using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using WalletContent;

namespace SoContent
{
    [CreateAssetMenu(fileName = "NewItemsConfig", menuName = "Configs/WorkerConfig")]
    public class WorkersConfig : ScriptableObject
    {
        [SerializeField] private List<WorkerConfig> _workerConfigs = new List<WorkerConfig>();
        
        public WorkerConfig GetWorkerConfig(WorkerType workerType)
        {
            foreach (var config in _workerConfigs)
            {
                if (config.WorkerType == workerType)
                    return config;
            }
            
            return null; 
        }
    }

    [Serializable]
    public class WorkerConfig
    {
        public WorkerType WorkerType;
        public Sprite SpriteIcon;
        public DollarValue Price;
        public DollarValue Salary;
    }
}