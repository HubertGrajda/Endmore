using Reflex.Core;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : MonoService<PlayerController>
    {
        private PlayerLocomotion _playerLocomotion;
        public PlayerLocomotion PlayerLocomotion => _playerLocomotion;
        
        private PlayerHealthSystem _playerHealthSystem;
        public PlayerHealthSystem PlayerHealthSystem => _playerHealthSystem;
        
        private PlayerInventory _playerInventory;
        public PlayerInventory PlayerInventory => _playerInventory;

        public override void InstallBindings(ContainerBuilder builder)
        {
            AssignComponents();

            base.InstallBindings(builder);

            if (_playerLocomotion)
            {
                builder.RegisterValue(_playerLocomotion);
            }

            if (_playerHealthSystem)
            {
                builder.RegisterValue(_playerHealthSystem);
            }

            if (_playerInventory)
            {
                builder.RegisterValue(_playerInventory);
            }
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