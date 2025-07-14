using System;
using Enums;
using UnityEngine;
using WalletContent;

namespace SoContent
{
    [CreateAssetMenu(fileName = "NewWorkerParametersConfig", menuName = "Configs/WorkerParametersConfig")]
    public class WorkerParametersConfig : ScriptableObject
    {
        [SerializeField] private WorkerTypeConfig[] _workerTypeConfigs;

        public WorkerParameterConfig GetConfig(WorkerType workerType, int level)
        {
            foreach (WorkerTypeConfig workerTypeConfig in _workerTypeConfigs)
            {
                if (workerTypeConfig.WorkerType == workerType)
                    return workerTypeConfig.GetConfig(level);
            }

            Debug.LogError($"Config for worker type {workerType} not found.");
            return null;
        }
    }

    [Serializable]
    public class WorkerTypeConfig
    {
        [SerializeField] private WorkerType _workerType;
        [SerializeField] private WorkerParameterConfig[] _workerParameterConfigs;

        public WorkerType WorkerType => _workerType;

        public WorkerParameterConfig GetConfig(int level)
        {
            foreach (WorkerParameterConfig workerParameterConfig in _workerParameterConfigs)
            {
                if (workerParameterConfig.Level == level)
                    return workerParameterConfig;
            }

            Debug.LogError($"Level {level} config for worker type {_workerType} not found.");
            return null;
        }
    }

    [Serializable]
    public class WorkerParameterConfig
    {
        [SerializeField] private int _level;
        [SerializeField] private float _delayWork;
        [SerializeField] private float _delayWorkNext;
        [SerializeField] private float _delayRelax;
        [SerializeField] private float _delayRelaxNext;
        [SerializeField] private float _speed;
        [SerializeField] private float _speedNext;
        [SerializeField] private float _efficiency;
        [SerializeField] private float _efficiencyNext;
        [SerializeField] private float _startSecondsEfficiency;
        [SerializeField] private DollarValue _priceUpgrade;

        public int MaxLevel { get; private set; } = 6;

        public int Level => _level;

        public float DelayWork => _delayWork;
        public float DelayWorkNext => _delayWorkNext;
        public float DelayRelax => _delayRelax;
        public float DelayRelaxNext => _delayRelaxNext;
        public float Speed => _speed;
        public float SpeedNext => _speedNext;
        public float Efficiency => _efficiency;
        public float EfficiencyNext => _efficiencyNext;
        public DollarValue PriceUpgrade => _priceUpgrade;
        public float StartSecondsEfficiency => _startSecondsEfficiency;
    }
}