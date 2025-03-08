using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Scripts.SaveSystem
{
    public class SaveManager : Singleton<SaveManager>
    {
        private static string SaveFilePath => $"{Application.persistentDataPath}/Save.json";
        
        private static Dictionary<string, Dictionary<string, SaveData>> _saveData;

        protected override void Awake()
        {
            base.Awake();
            Load();
        }

        [ContextMenu("Save")]
        public void Save()
        {
            _saveData ??= new Dictionary<string, Dictionary<string, SaveData>>();
            
            CaptureSaveHandlersState();
            SerializeSaveData();
        }

        [ContextMenu("Load")]
        public void Load()
        {
            if (!File.Exists(SaveFilePath))
            {
                _saveData ??= new Dictionary<string, Dictionary<string, SaveData>>();
                return;
            }
            
            DeserializeSaveData();
            RestoreSaveHandlersState();
        }

        [ContextMenu("Delete")]
        public void DeleteSave()
        {
            if (!File.Exists(SaveFilePath)) return;
            
            File.Delete(SaveFilePath);
            _saveData?.Clear();
        }

        private static void CaptureSaveHandlersState()
        {
            var saveHandlers = FindObjectsByType<SaveHandler>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            foreach (var saveHandler in saveHandlers)
            {
                SaveState(saveHandler.Id, saveHandler.Save());
            }
        }

        private static void RestoreSaveHandlersState()
        {
            var saveHandlers = FindObjectsByType<SaveHandler>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var saveHandler in saveHandlers)
            {
                if (!_saveData.TryGetValue(saveHandler.Id, out var saveData)) continue;
                
                saveHandler.Load(saveData);
            }
        }

        private static void SerializeSaveData()
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var json = JsonConvert.SerializeObject(_saveData, Formatting.Indented, settings);
            
            File.WriteAllText(SaveFilePath, json);
        }

        private static void DeserializeSaveData()
        {
            var json = File.ReadAllText(SaveFilePath);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            
            _saveData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, SaveData>>>(json, settings);
        }
        
        public static void SaveState(string id, Dictionary<string, SaveData> stateInfo)
        {
            if (string.IsNullOrEmpty(id)) return;
            
            _saveData[id] = stateInfo;
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.Save();
            Save();
        }
    }
}