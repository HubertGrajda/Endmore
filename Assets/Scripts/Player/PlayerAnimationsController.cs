using UnityEngine;

namespace Scripts.Player
{
    [RequireComponent(typeof(PlayerLocomotion))]
    public class PlayerAnimationsController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private PlayerLocomotion _playerLocomotion;
        
        private static readonly int HorizontalMovementAnimation = Animator.StringToHash("HorizontalMovement");
        private static readonly int VerticalMovementAnimation = Animator.StringToHash("VerticalMovement");

        private void Awake()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        private void OnEnable() => AddListeners();

        private void OnDisable() => RemoveListeners();
        
        private void AddListeners()
        {
            if (!animator) return;

            if (_playerLocomotion)
            {
                _playerLocomotion.OnMovementDirectionChanged += OnMovementDirectionChanged;
            }
        }

        private void RemoveListeners()
        {
            if (_playerLocomotion)
            {
                _playerLocomotion.OnMovementDirectionChanged -= OnMovementDirectionChanged;
            }
        }
        
        private void OnMovementDirectionChanged(Vector2 direction)
        {
            animator.SetFloat(HorizontalMovementAnimation, direction.x);
            animator.SetFloat(VerticalMovementAnimation, direction.y);
        }
    }
}