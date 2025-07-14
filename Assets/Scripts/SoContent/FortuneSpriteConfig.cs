using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoContent
{
    [CreateAssetMenu(fileName = "FortuneSpriteConfig", menuName = "Configs/FortuneSpriteConfig", order = 1)]
    public class FortuneSpriteConfig : ScriptableObject
    {
        [SerializeField] private List<FortunePrize> _fortunePrizes = new List<FortunePrize>();

        public Sprite GetSpriteByType(PrizesFortune type)
        {
            foreach (var prize in _fortunePrizes)
            {
                if (prize.Type == type)
                    return prize._sprite;
            }

            return null;
        }
    }
}

[Serializable]
public struct FortunePrize
{
    public PrizesFortune Type;
    public Sprite _sprite;
}