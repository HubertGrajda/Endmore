using System;
using System.Collections;
using Scripts.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerLocomotion : MonoBehaviour, IMovable, IKnockbackable
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private LayerMask collisionLayerMask;
        
        public event Action<Vector2> OnMovementDirectionChanged;
        
        private InputAction _movementInputAction;
        private PlayerHealthSystem _playerHealthSystem;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        
        private Vector2 _movementDirection;
        private float _currentMovementSpeed;
        private bool _isMovementBlocked;
        
        private Coroutine _knockbackCoroutine;
        private Coroutine _speedUpCoroutine;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            _currentMovementSpeed = movementSpeed;
            _movementInputAction = InputManager.Instance.PlayerInputs.Move;
            
            if (TryGetComponent(out _playerHealthSystem))
            {
                _playerHealthSystem.OnDeath += OnDeath;
            }
        }

        private void OnDeath()
        {
            _playerHealthSystem.OnDeath -= OnDeath;
            BlockMovementInput();
        }

        private void FixedUpdate()
        {
            HandleMovementInput();
        }

        private void HandleMovementInput()
        {
            if (_isMovementBlocked) return;
            
            ProcessMovementInputValue();
            
            if (_movementDirection == Vector2.zero) return;

            var targetPosition = GetTargetPositionForDirection();

            MoveTo(targetPosition);
        }

        private void ProcessMovementInputValue()
        {
            if (_movementInputAction == null) return;
            
             var movementDirection = _movementInputAction.ReadValue<Vector2>();

             if (movementDirection == _movementDirection) return;
             
             _movementDirection = movementDirection;
            OnMovementDirectionChanged?.Invoke(_movementDirection);
        }

        private Vector2 GetTargetPositionForDirection()
        {
            Vector2 direction = (transform.right * _movementDirection.x + transform.up * _movementDirection.y).normalized;
            return _rigidbody.position + direction * (_currentMovementSpeed * Time.fixedDeltaTime);
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
            if (_knockbackCoroutine != null)
            {
                StopCoroutine(_knockbackCoroutine);
            }
            
            _knockbackCoroutine = StartCoroutine(KnockbackCoroutine(force, duration));
        }

        private IEnumerator KnockbackCoroutine(Vector2 force, float duration)
        {
            BlockMovementInput();
            
            var timer = 0f;
            
            while (timer < duration)
            {
                var strength = Mathf.Lerp(1f, 0f, timer / duration);
                var targetPosition = _rigidbody.position + force * (strength * Time.fixedDeltaTime);
                
                MoveTo(targetPosition);
                timer += Time.deltaTime;
                yield return null;
            }

            UnblockMovementInput();
        }

        public void SpeedUp(float speed, float duration)
        {
            if (_speedUpCoroutine != null)
            {
                StopCoroutine(_speedUpCoroutine);
            }
            
            _speedUpCoroutine = StartCoroutine(SpeedUpCoroutine(_currentMovementSpeed + speed, duration));
        }
        
        private IEnumerator SpeedUpCoroutine(float speed, float duration)
        {
            _currentMovementSpeed = speed;
            yield return new WaitForSeconds(duration);
            _currentMovementSpeed = movementSpeed;
        }

        private void BlockMovementInput() => _movementInputAction.Disable();

        private void UnblockMovementInput()
        {
            if (_playerHealthSystem && _playerHealthSystem.IsDead) return;
            
            _movementInputAction.Enable();
        }
    }
}