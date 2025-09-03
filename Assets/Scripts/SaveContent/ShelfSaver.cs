using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using ItemContent;
using LoadingSceneContent;
using ShelfContent;
using UnityEngine;

namespace SaveContent
{
    public class ShelfSaver : MonoBehaviour
    {
        [SerializeField] private Shelf _shelf;
        [SerializeField] private bool _isBuyed;
        [SerializeField] private int _index;
        [SerializeField] private LoadingGame _loadingGame;

        private List<ItemType> _itemTypes = new List<ItemType>();

        private void OnEnable()
        {
            _shelf.ListItemChanged += SaveDate;
            _loadingGame.MirraSDKInitialization += Init;
        }

        private void OnDisable()
        {
            _shelf.ListItemChanged -= SaveDate;
            _loadingGame.MirraSDKInitialization -= Init;
        }

        /*private void Start()
        {
            LoadDataFromPlayerPrefs();
        }*/

        private void Init()
        {
            LoadDataFromPlayerPrefs();
        }

        private void SaveDate(List<ItemBasket> itemBasketList, List<ItemDrinkPackage> itemDrinkList)
        {
            int[] combinedIndices = itemBasketList.Select(item => (int)item.ItemType)
                .Concat(itemDrinkList.Select(item => (int)item.ItemType))
                .ToArray();

            string combinedIndicesString = string.Join(",", combinedIndices);

            string key = !_isBuyed ? "combinedItemIndices" : "combinedItemIndices" + _index;

            StorageHelper.SetString(key, combinedIndicesString);
            
            /*int[] combinedIndices = itemBasketList.Select(item => (int)item.ItemType)
                .Concat(itemDrinkList.Select(item => (int)item.ItemType))
                .ToArray();

            string combinedIndicesString = string.Join(",", combinedIndices);

            if (!_isBuyed)
                PlayerPrefs.SetString("combinedItemIndices", combinedIndicesString);
            else
                PlayerPrefs.SetString("combinedItemIndices" + _index, combinedIndicesString);

            PlayerPrefs.Save();*/
        }

        private void LoadDataFromPlayerPrefs()
        {
            string key = !_isBuyed ? "combinedItemIndices" : "combinedItemIndices" + _index;

            string combinedIndicesString = StorageHelper.GetString(key, "");

            if (!string.IsNullOrEmpty(combinedIndicesString))
            {
                string[] indicesArray = combinedIndicesString.Split(',');
                int[] indices = Array.ConvertAll(indicesArray, int.Parse);

                foreach (var index in indices)
                    _itemTypes.Add((ItemType)index);
            }

            if (_itemTypes.Count > 0)
                _shelf.Initialization(_itemTypes);
            
            
            
            /*string combinedIndicesString;

            combinedIndicesString = !_isBuyed
                ? PlayerPrefs.GetString("combinedItemIndices", "")
                : PlayerPrefs.GetString("combinedItemIndices" + _index, "");

            if (!string.IsNullOrEmpty(combinedIndicesString))
            {
                string[] indicesArray = combinedIndicesString.Split(',');
                int[] indices = Array.ConvertAll(indicesArray, int.Parse);

                foreach (var index in indices)
                    _itemTypes.Add((ItemType)index);
            }

            if (_itemTypes.Count > 0)
                _shelf.Initialization(_itemTypes);*/
        }
    }
}