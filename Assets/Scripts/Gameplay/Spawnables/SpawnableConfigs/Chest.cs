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
        private readonly List<Spawnable> _spawnedContent = new();
        
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
            _spawnedContent.Clear();
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
                _spawnedContent.Add(spawnableInstance);
            }
            
            spriteRenderer.sprite = Config.ChestOpenedSprite;
            onChestOpened?.Invoke();
            _isOpened = true;
        }

        public override void Clear()
        {
            _spawnedContent.ForEach(spawnable => spawnable.Clear());
            base.Clear();
        }
    }
}