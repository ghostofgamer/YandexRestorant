using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ItemContent;
using MirraGames.SDK;
using UnityEngine;

namespace SaveContent
{
    public class BoxSaver : MonoBehaviour
    {
        [SerializeField] private BoxesCounter _boxesCounter;
        
        private const string BoxDataKey = "BoxData";
        
        /*private void Start()
        {
            LoadData();
        }*/

        /*private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveData();
        }

        private void  OnApplicationFocus(bool pauseStatus)
        {
            if (!pauseStatus)
                SaveData();
        }*/

        /*private void OnApplicationQuit()
        {
            SaveData();
        }*/
        
        public void SaveData()
        {
            /*List<BoxData> boxesToSave = _boxesCounter.ItemBaskets
                .Select(item =>
                {
                    Debug.Log($"ItemBasket: Type = {(int)item.ItemType}, ActiveValueItems = {item.GetActiveValueItems()}");
                    return new BoxData(
                        (int)item.ItemType,
                        item.transform.position,
                        item.GetActiveValueItems(),
                        item.IsAdditionalItemsBasket,
                        item.GetActiveValueArrayItems().ToList()
                    );
                })
                .Concat(_boxesCounter.ItemDrinkPackages
                    .Select(item =>
                    {
                        Debug.Log($"ItemDrinkPackage: Type = {(int)item.ItemType}, CurrentFullness = {item.CurrentFullness}");
                        return new BoxData(
                            (int)item.ItemType,
                            item.transform.position,
                            item.CurrentFullness,
                            false,
                            null
                        );
                    }))
                .ToList();
            string jsonData = JsonUtility.ToJson(new BoxDataWrapper(boxesToSave));
            string path = Path.Combine(Application.persistentDataPath, "boxData.json");
            File.WriteAllText(path, jsonData);*/
            
            
            List<BoxData> boxesToSave = _boxesCounter.ItemBaskets
                .Select(item =>
                {
                    Debug.Log($"ItemBasket: Type = {(int)item.ItemType}, ActiveValueItems = {item.GetActiveValueItems()}");
                    return new BoxData(
                        (int)item.ItemType,
                        item.transform.position,
                        item.GetActiveValueItems(),
                        item.IsAdditionalItemsBasket,
                        item.GetActiveValueArrayItems().ToList()
                    );
                })
                .Concat(_boxesCounter.ItemDrinkPackages
                    .Select(item =>
                    {
                        Debug.Log($"ItemDrinkPackage: Type = {(int)item.ItemType}, CurrentFullness = {item.CurrentFullness}");
                        return new BoxData(
                            (int)item.ItemType,
                            item.transform.position,
                            item.CurrentFullness,
                            false,
                            null
                        );
                    }))
                .ToList();

            // Оборачиваем в wrapper
            /*BoxDataWrapper wrapper = new BoxDataWrapper(boxesToSave);*/
            
            StorageHelper.SetObject(BoxDataKey, new BoxDataWrapper(boxesToSave));
            Debug.Log("Box data saved to Mirra SDK storage.");
        }
        
        /*public void SaveData()
        {
            if (_boxesCounter == null || _boxesCounter.ItemBaskets == null || _boxesCounter.ItemDrinkPackages == null)
            {
                Debug.LogError("BoxesCounter or its lists are null!");
                return;
            }

            List<BoxData> boxesToSave = _boxesCounter.ItemBaskets
                .Where(item => item != null)
                .Select(item => new BoxData((int)item.ItemType, item.transform.position, item.GetActiveValueItems(),
                    item.IsAdditionalItemsBasket, item.GetActiveValueArrayItems().ToList()))
                .Concat(_boxesCounter.ItemDrinkPackages
                    .Where(item => item != null)
                    .Select(item => new BoxData((int)item.ItemType, item.transform.position, item.CurrentFullness,
                        false, null)))
                .ToList();

            string jsonData = JsonUtility.ToJson(new BoxDataWrapper(boxesToSave));

            // Сохраняем JSON-строку через MirraSDK
            MirraSDK.Data.SetString("boxData", jsonData);
        }*/
        
         /*public async void SaveData()
        {
            try
            {
                // Преобразуем данные коробок в формат для сохранения
                List<BoxData> boxesToSave = _boxesCounter.ItemBaskets
                    .Select(item => new BoxData((int)item.ItemType, item.transform.position, item.GetActiveValueItems(),
                        item.IsAdditionalItemsBasket, item.GetActiveValueArrayItems().ToList()))
                    .Concat(_boxesCounter.ItemDrinkPackages
                        .Select(item => new BoxData((int)item.ItemType, item.transform.position, item.CurrentFullness,
                            false, null)))
                    .ToList();

                // Сериализуем данные в JSON
                string jsonData = JsonUtility.ToJson(new BoxDataWrapper(boxesToSave));
                string path = Path.Combine(Application.persistentDataPath, "boxData.json");

                // Асинхронная запись в файл
                await File.WriteAllTextAsync(path, jsonData);
                Debug.Log($"Data saved successfully to {path}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save data: {ex.Message}");
            }
        }*/


        public List<BoxData> LoadData()
        {
            /*// Загружаем данные из JSON файла
            string path = Application.persistentDataPath + "/boxData.json";

            string persistentDataPath = Application.persistentDataPath;
            Debug.Log("Persistent Data Path: " + persistentDataPath);

            if (string.IsNullOrWhiteSpace(persistentDataPath))
            {
                Debug.LogError("Persistent Data Path is empty or whitespace.");
                return new List<BoxData>();
            }

            if (!Directory.Exists(persistentDataPath))
            {
                Directory.CreateDirectory(persistentDataPath);
            }

            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                BoxDataWrapper wrapper = JsonUtility.FromJson<BoxDataWrapper>(jsonData);
                return wrapper.boxes;
            }

            return new List<BoxData>();*/
            
            
            
            return StorageHelper.HasKey(BoxDataKey) 
                ? StorageHelper.GetObject<BoxDataWrapper>(BoxDataKey).boxes 
                : new List<BoxData>();
            
            
            /*if (MirraSDK.Data.HasKey(BoxDataKey))
            {
                BoxDataWrapper wrapper = MirraSDK.Data.GetObject<BoxDataWrapper>(BoxDataKey);
                Debug.Log($"Loaded {wrapper.boxes.Count} boxes from Mirra SDK storage.");
                return wrapper.boxes;
            }

            Debug.Log("No saved box data found.");
            return new List<BoxData>();*/
        }
        
        /*public List<BoxData> LoadData()
        {
            // Проверяем, есть ли сохранённые данные
            if (!MirraSDK.Data.HasKey("boxData"))
            {
                Debug.Log("No saved box data found.");
                return new List<BoxData>();
            }

            // Получаем JSON-строку
            string jsonData = MirraSDK.Data.GetString("boxData");

            if (string.IsNullOrEmpty(jsonData))
            {
                Debug.LogError("Saved box data is empty or corrupted.");
                return new List<BoxData>();
            }

            // Десериализуем данные
            BoxDataWrapper wrapper = JsonUtility.FromJson<BoxDataWrapper>(jsonData);

            if (wrapper != null && wrapper.boxes != null)
            {
                return wrapper.boxes;
            }

            Debug.LogError("Failed to deserialize box data.");
            return new List<BoxData>();
        }*/

        [ContextMenu("ClearSavedData")]
        public void ClearSavedData()
        {
            /*_boxesCounter.Clear();
            // MirraSDK.Data.DeleteAll();

            string path = Application.persistentDataPath + "/boxData.json";
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Saved data cleared.");
            }
            else
            {
                Debug.Log("No saved data found.");
            }*/
            
            _boxesCounter.Clear();
            StorageHelper.DeleteKey(BoxDataKey);
        }
    }

    [System.Serializable]
    public struct BoxData
    {
        public int itemType;
        public Vector3 position;
        public int amount;
        public bool additional;
        public List<int> additionalAmountItems;

        public BoxData(int type, Vector3 pos, int amount, bool additional, List<int> addAmtItems)
        {
            itemType = type;
            position = pos;
            this.amount = amount;
            this.additional = additional;
            additionalAmountItems = addAmtItems;
        }
    }

    [System.Serializable]
    public class BoxDataWrapper
    {
        public List<BoxData> boxes;

        public BoxDataWrapper(List<BoxData> boxes)
        {
            this.boxes = boxes;
        }
    }
}