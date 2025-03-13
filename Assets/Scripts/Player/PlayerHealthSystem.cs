using System;
using Scripts.Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Player
{
    public class PlayerHealthSystem : MonoBehaviour, IDamagable, IHealable
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        
        [SerializeField] private UnityEvent onDamageTaken;
        
        public event Action OnDeath;
        public event Action<int> OnHealthChanged;
        
        private GameplayManager _gameplayManager;
        private ScoreManager _scoreManager;

        private bool _isDead;
        private bool _initialized;
        
        public int CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;
        
        private void Start()
        {
            _gameplayManager = GameplayManager.Instance;
            _scoreManager = ScoreManager.Instance;
            _scoreManager.OnScoreTargetAchieved += OnScoreTargetAchieved;
            SetHealth(MaxHealth);
        }

        private void OnDestroy()
        {
            _scoreManager.OnScoreTargetAchieved -= OnScoreTargetAchieved;
        }
        
        private void OnScoreTargetAchieved(int obj)
        {
            SetHealth(CurrentHealth+1);
        }

        public void TakeDamage(int damage)
        {
            if (_isDead) return;
            
            onDamageTaken?.Invoke();
            _gameplayManager.IncrementCollisionsNumber();
            
            SetHealth(CurrentHealth - damage);
            
            if (CurrentHealth <= 0)
            {
                Death();
            }
        }

        public void Heal(int healAmount)
        {
            SetHealth(CurrentHealth + healAmount);
        }
        
        private void SetHealth(int health)
        {
            if (CurrentHealth == health) return;
            
            CurrentHealth = Mathf.Clamp(health, 0, MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth);
        }
        
        private void Death()
        {
            if (_isDead) return;
        
            _isDead = true;
            OnDeath?.Invoke();
        }
    }
}