using System;
using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

namespace Scripts.SaveSystem
{
    [DisallowMultipleComponent]
    public class SaveHandler : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
        
        private ISaveableBase[] _saveables;
        private ISaveableBase[] Saveables => _saveables ??= GetComponents<ISaveableBase>();
        
        [Inject] private ISaveService _saveService;
        
        [ContextMenu("Generate Id")]
        public void GenerateId() => Id = Guid.NewGuid().ToString();

        private void Awake()
        {
            Validate();
        }

        private void Start()
        {
            if (string.IsNullOrWhiteSpace(Id)) return;

            _saveService.RegisterHandler(this);
        }

        private void OnDestroy()
        {
            _saveService?.UnRegisterHandler(this);
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
                var data = saveable.Save();
                
                if (data == null) continue; 
                    
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
    }
}