using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Chest : Spawnable<ChestConfig>, IInteractable
    {
        [SerializeField] private List<Transform> contentSlots;
        [SerializeField] private UnityEvent onChestOpened;
        
        private bool _isOpened;
        private readonly List<Spawnable> _spawnedContent = new();
        
        public bool CanInteract => !_isOpened;
        
        public void Interact(GameObject interactor) => Open();

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