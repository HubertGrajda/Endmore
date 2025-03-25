using System.Collections.Generic;
using Scripts.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Chest : Spawnable<ChestConfig>, IInteractable
    {
        [SerializeField] private List<Transform> contentSlots;
        [SerializeField] private UnityEvent onChestOpened;
        
        private bool _isOpened;
        private PlayerInventory _playerInventory;
        
        public bool CanInteract => !_isOpened && (Config.KeyItem == null || _playerInventory.Has(Config.KeyItem));

        public override void Initialize(SpawnableConfig config)
        {
            base.Initialize(config);
            _playerInventory = PlayerController.Instance.PlayerInventory;
        }

        public void Interact(GameObject interactor)
        {
            if (!CanInteract) return;
                
            Open();
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            _isOpened = false;
            spriteRenderer.sprite = Config.Sprite;
        }

        private void Open()
        {
            if (_isOpened) return;

            _playerInventory.Remove(Config.KeyItem);
            
            for (var i = 0; i < Config.Content.Count; i++)
            {
                if (i >= contentSlots.Count) break;
                
                var spawnable = Config.Content[i];
                
                var slot = contentSlots[i];
                
                var spawnableInstance = SpawnableFactory.SpawnFromPool(spawnable);
                spawnableInstance.transform.position = slot.position;
            }
            
            spriteRenderer.sprite = Config.ChestOpenedSprite;
            onChestOpened?.Invoke();
            _isOpened = true;
        }
    }
}