using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using GarbageContent;
using PlayerContent.LevelContent;
using TutorialContent;
using UnityEngine;
using Random = System.Random;

namespace RestaurantContent.TableContent
{
    public class TableCleanliness : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private GarbagePackage[] _garbagePackages;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Transform _cleanerPosition;
        [SerializeField] private Transform _lookDirtyPosition;

        private int _maxPollutionLevel = 3;

        public event Action<TableCleanliness> TablePolluted;
        public event Action<TableCleanliness> TableCleaned;

        public int PollutionLevel { get; private set; }
        public Transform CleanerPosition => _cleanerPosition;
        public Transform LookDirtyPosition => _lookDirtyPosition;

        [ContextMenu("PolluteTable")]
        public void PolluteTable()
        {
            if (_garbagePackages.Length <= 0) return;

            if (PollutionLevel >= _maxPollutionLevel) return;

            PollutionLevel++;
            // _dirtyCounter.AddDirtyTable(this);
            TablePolluted?.Invoke(this);

            Random random = new Random();
            List<GarbagePackage> garbagesTable = _garbagePackages.Where(t => !t.IsActive).ToList();

            if (garbagesTable.Count > 0)
            {
                int randomIndex = random.Next(garbagesTable.Count);

                garbagesTable[randomIndex].SetValue(true);
            }
        }

        public int GetTrashActiveCount()
        {
            int amount = 0;

            foreach (var garbage in _garbagePackages)
            {
                if (garbage.gameObject.activeSelf)
                    amount++;
            }

            return amount;
        }

        public void DecreasePollutionLevel()
        {
            if (PollutionLevel <= 0) return;

            /*if (_tutorial != null)
            {
                if (_tutorial.CurrentType == TutorialType.CleanTable)
                    _tutorial.SetCurrentTutorialStage(TutorialType.CleanTable);
            }*/

            PollutionLevel--;
            _playerLevel.AddExp(5);

            if (PollutionLevel == 0)
                TableCleaned?.Invoke(this);
        }

        public void ClearTable()
        {
            if (PollutionLevel <= 0) return;
            PollutionLevel = 0;
            DeactivateGarbages();

            TableCleaned?.Invoke(this);
        }

        private void DeactivateGarbages()
        {
            foreach (var garbage in _garbagePackages)
                garbage.SetValue(false);
        }
    }
}