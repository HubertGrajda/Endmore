using UnityEngine;

namespace Scripts.Player
{
    [RequireComponent(typeof(PlayerLocomotion))]
    [RequireComponent(typeof(PlayerHealthSystem))]
    public class PlayerAnimationsController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private PlayerLocomotion _playerLocomotion;
        private PlayerHealthSystem _playerHealthSystem;
        
        private static readonly int HorizontalMovementAnimation = Animator.StringToHash("HorizontalMovement");
        private static readonly int VerticalMovementAnimation = Animator.StringToHash("VerticalMovement");
        private static readonly int DeathAnimation = Animator.StringToHash("Death");

        private void Awake()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _playerHealthSystem = GetComponent<PlayerHealthSystem>();
        }

        private void OnEnable() => AddListeners();

        private void OnDisable() => RemoveListeners();
        
        private void AddListeners()
        {
            if (!animator) return;

            _playerLocomotion.OnMovementDirectionChanged += OnMovementDirectionChanged;
            _playerHealthSystem.OnDeath += OnDeath;
        }


        private void RemoveListeners()
        {
            _playerLocomotion.OnMovementDirectionChanged -= OnMovementDirectionChanged;
            _playerHealthSystem.OnDeath -= OnDeath;
        }
        
        private void OnDeath()
        {
            animator.SetTrigger(DeathAnimation);
        }
        
        private void OnMovementDirectionChanged(Vector2 direction)
        {
            animator.SetFloat(HorizontalMovementAnimation, direction.x);
            animator.SetFloat(VerticalMovementAnimation, direction.y);
        }
    }
}