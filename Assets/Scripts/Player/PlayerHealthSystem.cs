using System;
using Reflex.Attributes;
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
        
        [Inject] private IGameplayService _gameplayManager;
        [Inject] private IScoreService _scoreService;

        private bool _isDead;
        private bool _initialized;
        
        public int CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;
        
        private void Start()
        {
            SetHealth(MaxHealth);
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
        
        private void AddListeners()
        {
            _scoreService.OnScoreTargetAchieved += OnScoreTargetAchieved;
        }

        private void RemoveListeners()
        {
            _scoreService.OnScoreTargetAchieved -= OnScoreTargetAchieved;
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
            _gameplayManager.FinishAndRestart();
        }
    }
}