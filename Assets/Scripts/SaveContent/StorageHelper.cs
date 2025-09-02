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
    }
}