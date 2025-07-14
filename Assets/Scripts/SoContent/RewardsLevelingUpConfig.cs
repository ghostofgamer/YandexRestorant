using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace SoContent
{
    [CreateAssetMenu(fileName = "NewRewardsLevelingUpConfig", menuName = "Configs/RewardsLevelingUp")]
    public class RewardsLevelingUpConfig : ScriptableObject
    {
        public List<RewardLeveling> levels;

        public RewardLeveling GetLevelData(int level)
        {
            return levels.Find(data => data.playerLevel == level);
        }
    }

    [Serializable]
    public class RewardLeveling
    {
        public int playerLevel;
        public List<RewardLevelingType> products;
        public List<RewardLevelingType> recipes;
        public List<RewardLevelingType> equipment;
    }
}