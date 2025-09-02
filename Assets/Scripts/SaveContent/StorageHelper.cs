using MirraGames.SDK;
using UnityEngine;

namespace SaveContent
{
    public static class StorageHelper
    {
        // Сохраняем int
        public static void SetInt(string key, int value, bool saveImmediately = true)
        {
            MirraSDK.Data.SetInt(key, value, true);
            if (saveImmediately) MirraSDK.Data.Save();
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            return MirraSDK.Data.HasKey(key) ? MirraSDK.Data.GetInt(key) : defaultValue;
        }

        // Сохраняем bool
        public static void SetBool(string key, bool value, bool saveImmediately = true)
        {
            MirraSDK.Data.SetInt(key, value ? 1 : 0, true);
            if (saveImmediately) MirraSDK.Data.Save();
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            return MirraSDK.Data.HasKey(key) ? MirraSDK.Data.GetInt(key) == 1 : defaultValue;
        }

        // Сохраняем строку
        public static void SetString(string key, string value, bool saveImmediately = true)
        {
            MirraSDK.Data.SetString(key, value, true);
            if (saveImmediately) MirraSDK.Data.Save();
        }

        public static string GetString(string key, string defaultValue = "")
        {
            return MirraSDK.Data.HasKey(key) ? MirraSDK.Data.GetString(key) : defaultValue;
        }

        // Сохраняем объект (через JSON)
        public static void SetObject<T>(string key, T obj, bool saveImmediately = true)
        {
            MirraSDK.Data.SetObject(key, obj, true);
            if (saveImmediately) MirraSDK.Data.Save();
        }

        public static T GetObject<T>(string key, T defaultValue = default)
        {
            return MirraSDK.Data.HasKey(key) ? MirraSDK.Data.GetObject<T>(key) : defaultValue;
        }
        
        public static void SetFloat(string key, float value, bool saveImmediately = true)
        {
            // Сохраняем float как double через JSON, если MirraSDK не поддерживает float напрямую
            SetString(key, value.ToString(System.Globalization.CultureInfo.InvariantCulture), saveImmediately);
        }

// Получаем float
        public static float GetFloat(string key, float defaultValue = 0f)
        {
            if (HasKey(key))
            {
                string str = GetString(key, defaultValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (float.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float result))
                    return result;
            }
            return defaultValue;
        }

        // Проверка и удаление
        public static bool HasKey(string key) => MirraSDK.Data.HasKey(key);

        public static void DeleteKey(string key, bool saveImmediately = true)
        {
            if (MirraSDK.Data.HasKey(key))
            {
                MirraSDK.Data.DeleteKey(key);
                if (saveImmediately) MirraSDK.Data.Save();
            }
        }

        public static void DeleteAll(bool saveImmediately = true)
        {
            MirraSDK.Data.DeleteAll();
            if (saveImmediately) MirraSDK.Data.Save();
        }
        
        public static void Save()
        {
            MirraSDK.Data.Save();
        }
    }
}