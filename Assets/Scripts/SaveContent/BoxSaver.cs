using System.Collections.Generic;
using System.IO;
using System.Linq;
using ItemContent;
using UnityEngine;

namespace SaveContent
{
    public class BoxSaver : MonoBehaviour
    {
        [SerializeField] private BoxesCounter _boxesCounter;

        private void Start()
        {
            LoadData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        public void SaveData()
        {
            // Преобразуем данные коробок в формат для сохранения
            List<BoxData> boxesToSave = _boxesCounter.ItemBaskets
                .Select(item => new BoxData((int)item.ItemType, item.transform.position, item.GetActiveValueItems(),
                    item.IsAdditionalItemsBasket, item.GetActiveValueArrayItems().ToList()))
                .Concat(_boxesCounter.ItemDrinkPackages
                    .Select(item => new BoxData((int)item.ItemType, item.transform.position, item.CurrentFullness,
                        false, null)))
                .ToList();

            // Сохраняем данные в JSON файл
            string jsonData = JsonUtility.ToJson(new BoxDataWrapper(boxesToSave));
            string path = Application.persistentDataPath + "/boxData.json";
            File.WriteAllText(path, jsonData);
        }


        public List<BoxData> LoadData()
        {
            // Загружаем данные из JSON файла
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

            return new List<BoxData>();
        }

        [ContextMenu("ClearSavedData")]
        public void ClearSavedData()
        {
            _boxesCounter.Clear();

            string path = Application.persistentDataPath + "/boxData.json";
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Saved data cleared.");
            }
            else
            {
                Debug.Log("No saved data found.");
            }
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