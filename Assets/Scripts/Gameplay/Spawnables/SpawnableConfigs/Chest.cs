using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Chest : Spawnable, IInteractable
    {
        [SerializeField] private List<Transform> contentSlots;
        [SerializeField] private UnityEvent onChestOpened;
        
        private ChestConfig _chestConfig;
        private bool _isOpened;
        
        public override void Initialize(SpawnableConfig config)
        {
            base.Initialize(config);
            
            if (config is not ChestConfig chestConfig) return;
            
            _chestConfig = chestConfig;
        }

        public void Interact(GameObject interactor) => Open();

        public override void OnDespawn()
        {
            base.OnDespawn();
            _isOpened = false;
            spriteRenderer.sprite = _chestConfig.Sprite;
        }

        private void Open()
        {
            if (_isOpened) return;

            for (var i = 0; i < _chestConfig.Content.Count; i++)
            {
                if (i >= contentSlots.Count) break;
                
                var spawnable = _chestConfig.Content[i];
                
                var slot = contentSlots[i];
                
                var spawnableInstance = SpawnableFactory.SpawnFromPool(spawnable);
                spawnableInstance.transform.position = slot.position;
            }
            
            spriteRenderer.sprite = _chestConfig.ChestOpenedSprite;
            _isOpened = true;
        }
    }
}