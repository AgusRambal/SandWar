using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace SandWar.Serialization
{
    public class SerializationManager : MonoBehaviour
    {
        private const string SAVE_DATA_KEY = "SandWarSaveData";

        public static void SaveData<T>(string key, T data)
        {
            Dictionary<string, object> saveData = LoadSaveData();

            if (saveData.ContainsKey(key))
            {
                saveData[key] = data;
            }
            else
            {
                saveData.Add(key, data);
            }

            SaveToFile(saveData);
        }

        public static bool TryLoadData<T>(string key, out T data)
        {
            Dictionary<string, object> saveData = LoadSaveData();

            if (saveData.ContainsKey(key))
            {
                object loadedData = saveData[key];

                if (loadedData is T)
                {
                    data = (T)loadedData;
                    return true;
                }
                else
                {
                    throw new InvalidCastException($"Cant cast {loadedData.GetType()} to {typeof(T)}"); // Aca en un futuro hay que desarrollar un sistema de manejo de excepciones 
                }
            }

            data = default;
            return false;
        }

        public static T TryLoadData<T>(string key, T defaultValue = default)
        {
            Dictionary<string, object> saveData = LoadSaveData();

            if (saveData.ContainsKey(key))
            {
                object loadedData = saveData[key];

                if (loadedData is T)
                {
                    return (T)loadedData;
                }
                else
                {
                    throw new InvalidCastException($"Cant cast {loadedData.GetType()} to {typeof(T)}"); // Aca en un futuro hay que desarrollar un sistema de manejo de excepciones 
                }
            }

            return defaultValue;
        }

        private static void SaveToFile(Dictionary<string, object> saveData)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(GetSaveFilePath(), FileMode.Create);
            formatter.Serialize(fileStream, saveData);
            fileStream.Close();
        }

        private static string GetSaveFilePath()
        {
            return Path.Combine(Application.persistentDataPath, SAVE_DATA_KEY + ".dat");
        }

        private static Dictionary<string, object> LoadSaveData()
        {
            if (File.Exists(GetSaveFilePath()))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(GetSaveFilePath(), FileMode.Open);
                Dictionary<string, object> saveData = formatter.Deserialize(fileStream) as Dictionary<string, object>;
                fileStream.Close();
                return saveData;
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }

    }
}

