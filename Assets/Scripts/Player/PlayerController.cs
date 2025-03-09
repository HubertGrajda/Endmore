using UnityEngine;

namespace Scripts.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        private PlayerLocomotion _playerLocomotion;
        public PlayerLocomotion PlayerLocomotion => _playerLocomotion;
        
        private PlayerHitsHandler _playerHitsHandler;
        public PlayerHitsHandler PlayerHitsHandler => _playerHitsHandler;

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
            
            if (!TryGetComponent(out _playerHitsHandler))
            {
                Debug.LogError($"{name}: Missing component: {nameof(PlayerHitsHandler)}");
            }
        }
    }
}