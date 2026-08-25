using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Scripts.SaveSystem
{
    public class SaveService : ISaveService
    {
        private bool _isSaving;
        private bool _saveRequestedDuringSave;

        private Dictionary<string, Dictionary<string, SaveData>> _saveData = new();

        private readonly Dictionary<string, SaveHandler> _handlers = new();
        private readonly JsonSerializerSettings _jsonSettings;

        private readonly string _saveFilePath;
        private readonly string _tempFilePath;
        private readonly string _backupFilePath;

        public SaveService()
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, "Save.json");
            _tempFilePath = _saveFilePath + ".tmp";
            _backupFilePath = _saveFilePath + ".bak";
            _jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
        }

        public void Save()
        {
            if (_isSaving)
            {
                _saveRequestedDuringSave = true;
                return;
            }

            CaptureSaveHandlersState();
            _ = SerializeAndSaveAsync();
        }

        public void Load()
        {
            if (File.Exists(_saveFilePath))
            {
                DeserializeSaveData();
            }
            else if (File.Exists(_backupFilePath))
            {
                DeserializeBackupSaveData();
            }
            else
            {
                _saveData = new Dictionary<string, Dictionary<string, SaveData>>();
                return;
            }

            RestoreSaveHandlersState();
        }

        public void DeleteSave()
        {
            if (File.Exists(_saveFilePath))
                File.Delete(_saveFilePath);

            if (File.Exists(_backupFilePath))
                File.Delete(_backupFilePath);

            _saveData.Clear();
        }

        private void CaptureSaveHandlersState()
        {
            foreach (var (id, handler) in _handlers)
            {
                if (handler == null) continue;

                CaptureState(id, handler.Save());
            }
        }

        private void RestoreSaveHandlersState()
        {
            foreach (var (id, handler) in _handlers)
            {
                if (handler == null) continue;
                if (!_saveData.TryGetValue(id, out var saveData)) continue;

                handler.Load(saveData);
            }
        }

        private async Task SerializeAndSaveAsync()
        {
            _isSaving = true;

            try
            {
                var snapshot = GetCurrentSaveSnapshot();
                var json = await Task.Run(() =>
                    JsonConvert.SerializeObject(snapshot, Formatting.Indented, _jsonSettings));

                await File.WriteAllTextAsync(_tempFilePath, json);

                if (File.Exists(_saveFilePath))
                {
                    File.Replace(_tempFilePath, _saveFilePath, _backupFilePath, true);
                }
                else
                {
                    File.Move(_tempFilePath, _saveFilePath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Saving Failed: {ex.Message}");
            }
            finally
            {
                _isSaving = false;
                if (_saveRequestedDuringSave)
                {
                    _saveRequestedDuringSave = false;
                    Save();
                }
            }
        }

        private Dictionary<string, Dictionary<string, SaveData>> GetCurrentSaveSnapshot()
        {
            var snapshot = new Dictionary<string, Dictionary<string, SaveData>>(_saveData.Count);
            foreach (var kvp in _saveData)
            {
                snapshot[kvp.Key] = new Dictionary<string, SaveData>(kvp.Value);
            }

            return snapshot;
        }

        private void DeserializeSaveData()
        {
            try
            {
                var json = File.ReadAllText(_saveFilePath);
                _saveData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, SaveData>>>(json, _jsonSettings) 
                            ?? new Dictionary<string, Dictionary<string, SaveData>>();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Loading failed: {ex.Message}");
                DeserializeBackupSaveData();
            }
        }

        private void DeserializeBackupSaveData()
        {
            try
            {
                var json = File.ReadAllText(_backupFilePath);
                _saveData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, SaveData>>>(json, _jsonSettings) 
                            ?? new Dictionary<string, Dictionary<string, SaveData>>();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Loading failed: {ex.Message}");
                _saveData = new Dictionary<string, Dictionary<string, SaveData>>();
            }
        }

        public void CaptureState(string id, Dictionary<string, SaveData> stateInfo)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (stateInfo == null) return;

            _saveData[id] = stateInfo;
        }

        public void RegisterHandler(SaveHandler handler)
        {
            if (handler == null) return;
            if (string.IsNullOrWhiteSpace(handler.Id)) return;
            if (!_handlers.TryAdd(handler.Id, handler)) return;

            if (_saveData.TryGetValue(handler.Id, out var savedState))
            {
                handler.Load(savedState);
            }
        }

        public void UnRegisterHandler(SaveHandler handler)
        {
            if (handler == null) return;
            if (string.IsNullOrWhiteSpace(handler.Id)) return;
            if (!_handlers.ContainsKey(handler.Id)) return;

            CaptureState(handler.Id, handler.Save());
            _handlers.Remove(handler.Id);
        }
    }
}