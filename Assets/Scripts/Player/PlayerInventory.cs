using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<ItemRestrictions> restrictions;
        
        [Serializable]
        private class ItemRestrictions
        {
            [field: SerializeField] public ItemConfig Config { get; private set; }
            [field: SerializeField] public int MaxAmount { get; private set; }
        }
        
        public event Action<ItemConfig> OnItemAdded;
        public event Action<ItemConfig> OnItemRemoved;
        
        private readonly Dictionary<ItemConfig, int> _collectedItems = new();
        
        public bool Has(ItemConfig itemConfig, int amount = 1) => 
            _collectedItems.TryGetValue(itemConfig, out var currentAmount) && currentAmount >= amount;

        public bool TryAdd(ItemConfig itemConfig, int amount = 1)
        {
            var restriction = restrictions.FirstOrDefault(x => x.Config == itemConfig);

            if (restriction != null)
            {
                var maxAmountToAdd = restriction.MaxAmount - _collectedItems.GetValueOrDefault(itemConfig, 0);
                
                if (maxAmountToAdd <= 0) return false;
                
                amount = Mathf.Clamp(amount, 0, maxAmountToAdd);
            }
            
            Add(itemConfig, amount);
            return true;
        }
        
        private void Add(ItemConfig itemConfig, int amount = 1)
        {
            if (!_collectedItems.TryAdd(itemConfig, amount))
            {
                _collectedItems[itemConfig] += amount;
            }

            OnItemAdded?.Invoke(itemConfig);
        }

        public void Remove(ItemConfig itemConfig)
        {
            if (!_collectedItems.TryGetValue(itemConfig, out var currentAmount)) return;
            if (currentAmount <= 0) return;

            _collectedItems[itemConfig]--;
            OnItemRemoved?.Invoke(itemConfig);
        }
    }
}