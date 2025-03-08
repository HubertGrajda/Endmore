using System;
using System.Collections;
using Scripts.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerLocomotion : MonoBehaviour, IKnockbackable
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private LayerMask collisionLayerMask;
        
        private InputAction _movementInputAction;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Vector2 _movementDirection;
        
        private bool _isMovementBlocked;
        
        public event Action<Vector2> OnMovementDirectionChanged;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            _movementInputAction = InputManager.Instance.PlayerInputs.Move;
        }

        private void FixedUpdate()
        {
            HandleMovementInput();
        }

        private void HandleMovementInput()
        {
            if (_isMovementBlocked) return;
            if (_movementInputAction == null) return;
            
            var movementDirection = _movementInputAction.ReadValue<Vector2>();

            if (movementDirection != _movementDirection)
            {
                _movementDirection = movementDirection;
                OnMovementDirectionChanged?.Invoke(_movementDirection);
            }
            
            if (movementDirection == Vector2.zero) return;

            Vector2 direction = (transform.right * movementDirection.x + transform.up * movementDirection.y).normalized;
            var targetPosition = _rigidbody.position + direction * (movementSpeed * Time.fixedDeltaTime);

            MoveTo(targetPosition);
        }

        private void MoveTo(Vector3 targetPosition)
        {
            var movementX = new Vector3(targetPosition.x, _rigidbody.position.y);
            var movementY = new Vector3(_rigidbody.position.x, targetPosition.y);
            
            var canMoveX = !Physics2D.OverlapBox(movementX, _collider.bounds.size, 0,  collisionLayerMask);
            var canMoveY = !Physics2D.OverlapBox(movementY, _collider.bounds.size,  0,collisionLayerMask);

            if (!canMoveX && !canMoveY) return;
            
            var positionToMove = canMoveX && canMoveY 
                ? targetPosition
                : canMoveX ? movementX : movementY;

            _rigidbody.MovePosition(positionToMove);
        }
        
        public void ApplyKnockback(Vector2 force, float duration)
        {
            StopAllCoroutines();
            StartCoroutine(KnockbackCoroutine(force, duration));
        }

        private IEnumerator KnockbackCoroutine(Vector2 force, float duration)
        {
            var timer = 0f;
            BlockInputMovement();
            
            while (timer < duration)
            {
                var strength = Mathf.Lerp(1f, 0f, timer / duration);
                var targetPosition = _rigidbody.position + force * (strength * Time.fixedDeltaTime);
                
                MoveTo(targetPosition);
                timer += Time.deltaTime;
                yield return null;
            }

            UnblockInputMovement();
        }

        private void BlockInputMovement() => _movementInputAction.Disable();
        
        private void UnblockInputMovement() => _movementInputAction.Enable();
    }
}