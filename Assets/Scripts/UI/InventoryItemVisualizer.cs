using Scripts.Gameplay;
using Scripts.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.UI
{
    public class InventoryItemVisualizer : MonoBehaviour
    {
        [SerializeField] private ItemConfig requiredItem;
        [SerializeField] private int requiredAmount = 1;
        
        [SerializeField] private UnityEvent onActivation;
        [SerializeField] private UnityEvent onDeactivation;

        private PlayerInventory _playerInventory;
        private bool _isActive;
        
        private void Activate()
        {
            if (_isActive) return;
            
            _isActive = true;
            onActivation?.Invoke();
        }

        private void Deactivate()
        {
            if (!_isActive) return;
            
            _isActive = false;
            onDeactivation?.Invoke();
        }
        
        private void Start()
        {
            _playerInventory = PlayerController.Instance.PlayerInventory;
            AddListeners();
        }
        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _playerInventory.OnItemAdded += OnItemCountChanged;
            _playerInventory.OnItemRemoved += OnItemCountChanged;
        }
        
        private void RemoveListeners()
        {
            _playerInventory.OnItemAdded -= OnItemCountChanged;
            _playerInventory.OnItemRemoved -= OnItemCountChanged;
        }

        private void OnItemCountChanged(ItemConfig item)
        {
            if (item != requiredItem) return;
            
            Refresh();
        }

        private void Refresh()
        {
            var activate = _playerInventory.Has(requiredItem, requiredAmount);
            
            if (activate && !_isActive)
            {
                Activate();
                return;
            }
            
            if (!activate && _isActive)
            {
                Deactivate();
            }
        }
    }
}