using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.SaveSystem
{
    public class SaveHandler : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
        
        private ISaveableBase[] _saveables;
        private ISaveableBase[] Saveables => _saveables ??= GetComponents<ISaveableBase>();

        private SaveManager _saveManager;
        
        [ContextMenu("Generate Id")]
        public void GenerateId() => Id = Guid.NewGuid().ToString();

        private void Awake()
        {
            _saveManager = SaveManager.Instance;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Debug.LogError("Id cannot be empty");
            }
        }
        
        public Dictionary<string, SaveData> Save()
        {
            var saveData = new Dictionary<string, SaveData>();
            
            foreach (var saveable in Saveables)
            {
                saveData[saveable.SaveKey] = saveable.Save();
            }

            return saveData;
        }

        public void Load(Dictionary<string, SaveData> data)
        {
            foreach (var saveable in Saveables)
            {
                if (!data.TryGetValue(saveable.SaveKey, out var saveData)) continue;
                
                saveable.Load(saveData);
            }
        }

        private void OnDestroy()
        {
            if (!_saveManager) return;
            
            SaveManager.SaveState(Id, Save());
        }
    }
}