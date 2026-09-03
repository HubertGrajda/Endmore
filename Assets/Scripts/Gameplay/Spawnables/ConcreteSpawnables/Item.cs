using Reflex.Attributes;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class Item : Spawnable<ItemConfig>, IInteractable
    {
        [Inject] private PlayerInventory _playerInventory;
        
        public void Interact(GameObject interactor)
        {
            if (!_playerInventory.TryAdd(Config)) return;
            
            Clear();
        }
    }
}