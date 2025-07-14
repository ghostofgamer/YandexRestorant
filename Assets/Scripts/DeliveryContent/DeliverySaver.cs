using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace DeliveryContent
{
    public class DeliverySaver : MonoBehaviour
    {
        private const string SavedItemsKey = "DeliverySaveData";
        private const string LastExitTimeKey = "LastExitTime";
        private const string RemainingTimeKey = "RemainingTimeKey";
    
        [SerializeField] private DeliveryViewer _deliveryViewer;
        [SerializeField] private Delivery _delivery;
        
        private void OnApplicationQuit()
        {
            Debug.Log("сохраняем при выходе");
            SaveLastExitTime();
            SaveDeliveryData();
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                LoadDeliveryData();
            }
            else
            {
                SaveLastExitTime();
                SaveDeliveryData();
            }
        }
        
        public void SaveLastExitTime()
        {
            PlayerPrefs.SetString(LastExitTimeKey, DateTime.UtcNow.ToString("O"));
            PlayerPrefs.SetFloat(RemainingTimeKey, _delivery.RemainingTime);
            PlayerPrefs.Save();
           
        }
    
        public void SaveDeliveryData()
        {
            try
            {
                var saveData = new DeliverySaveData
                {
                    Items = _delivery.CurrentItems.Select(i => new SavedItemData(i.ItemType, i.Amount)).ToList(),
                    RemainingTime = _delivery.RemainingTime,
                    SaveTime = DateTime.UtcNow.ToString("O")
                };
                
                string json = JsonUtility.ToJson(saveData);
                PlayerPrefs.SetString(SavedItemsKey, json);
                PlayerPrefs.Save();
            }
            catch
            {
                /* обработка ошибок */
            }
        }
    
        public void LoadDeliveryData()
        {
            try
            {
                if (!PlayerPrefs.HasKey(SavedItemsKey))
                    return;

                string json = PlayerPrefs.GetString(SavedItemsKey);
                var saveData = JsonUtility.FromJson<DeliverySaveData>(json);
                
                DateTime saveTime = DateTime.Parse(saveData.SaveTime).ToUniversalTime();
                _delivery.Init(ConvertFromSaveData(saveData),saveData.RemainingTime,saveTime);
            }
            catch
            {
                _delivery.SetItemsList(new List<ItemDeliveryInfo>());
               
            }
        }
    
        private List<ItemDeliveryInfo> ConvertFromSaveData(DeliverySaveData saveData)
        {
            var result = new List<ItemDeliveryInfo>();

            if (saveData?.Items != null)
            {
                foreach (var savedItem in saveData.Items)
                {
                    result.Add(new ItemDeliveryInfo(
                        (ItemType)savedItem.ItemTypeInt,
                        savedItem.Amount
                    ));
                }
            }

            return result;
        }
        
        private DeliverySaveWrapper ConvertToSaveData()
        {
            var wrapper = new DeliverySaveWrapper
            {
                SaveTime = DateTime.UtcNow.ToString("O")
            };

            foreach (var item in _delivery.CurrentItems)
            {
                wrapper.Items.Add(new SavedItemData(item.ItemType, item.Amount));
            }

            return wrapper;
        }
        
        [System.Serializable]
        private class DeliverySaveData
        {
            public List<SavedItemData> Items = new List<SavedItemData>();
            public float RemainingTime;
            public string SaveTime;
        }
    
        [System.Serializable]
        private class SavedItemData
        {
            public int ItemTypeInt;
            public int Amount;

            public SavedItemData(ItemType type, int amount)
            {
                ItemTypeInt = (int)type;
                Amount = amount;
            }
        }
        
        [System.Serializable]
        private class ItemListWrapper
        {
            public List<ItemDeliveryInfo> Items;
        }

        [System.Serializable]
        private class DeliverySaveWrapper
        {
            public List<SavedItemData> Items = new List<SavedItemData>();
            public string SaveTime;
        }
    }
}