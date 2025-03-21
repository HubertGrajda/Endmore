using Scripts.Player;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class Item : Spawnable<ItemConfig>, IInteractable
    {
        private PlayerInventory _playerInventory;
        
        public override void Initialize(SpawnableConfig config)
        {
            base.Initialize(config);
            _playerInventory = PlayerController.Instance.PlayerInventory;
        }

        public void Interact(GameObject interactor)
        {
            if (!_playerInventory.TryAdd(Config)) return;
            
            Clear();
        }
    }
}