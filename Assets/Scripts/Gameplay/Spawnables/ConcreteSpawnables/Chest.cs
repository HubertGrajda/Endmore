using System.Collections.Generic;
using Reflex.Attributes;
using Scripts.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Chest : Spawnable<ChestConfig>, IInteractable
    {
        [SerializeField] private List<Transform> contentSlots;
        [SerializeField] private UnityEvent onChestOpened;
        
        [Inject] private PlayerInventory _playerInventory;
        
        private bool _isOpened;
        
        public bool CanInteract => !_isOpened && (Config.KeyItem == null || _playerInventory.Has(Config.KeyItem));

        public void Interact(GameObject interactor)
        {
            if (!CanInteract) return;
                
            Open();
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            _isOpened = false;
            SpriteRenderer.sprite = Config.Sprite;
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
                
                var spawnableInstance = Factory.SpawnFromPool(spawnable);
                spawnableInstance.transform.position = slot.position;
            }
            
            SpriteRenderer.sprite = Config.ChestOpenedSprite;
            onChestOpened?.Invoke();
            _isOpened = true;
        }
    }
}