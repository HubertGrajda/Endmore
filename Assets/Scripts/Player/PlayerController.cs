using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        private PlayerLocomotion _playerLocomotion;
        public PlayerLocomotion PlayerLocomotion => _playerLocomotion;
        
        private PlayerHealthSystem _playerHealthSystem;
        public PlayerHealthSystem PlayerHealthSystem => _playerHealthSystem;
        
        private PlayerInventory _playerInventory;
        public PlayerInventory PlayerInventory => _playerInventory;

        protected override void Awake()
        {
            base.Awake();
            AssignComponents();
        }

        private void AssignComponents()
        {
            if (!TryGetComponent(out _playerLocomotion))
            {
                Debug.LogError($"{name}: Missing component: {nameof(PlayerLocomotion)}");
            }
            
            if (!TryGetComponent(out _playerHealthSystem))
            {
                Debug.LogError($"{name}: Missing component: {nameof(PlayerHealthSystem)}");
            }
            
            if (!TryGetComponent(out _playerInventory))
            {
                Debug.LogError($"{name}: Missing component: {nameof(PlayerInventory)}");
            }
        }
    }
}