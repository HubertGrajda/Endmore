using System.Collections.Generic;
using Scripts.Player;
using UnityEngine;

namespace Scripts.UI
{
    public class PlayerHealthVisualizer : MonoBehaviour
    {
        [SerializeField] private HealthPointVisualizer healthPointPrefab;
        [SerializeField] private Transform healthPointsContainer;

        private PlayerHealthSystem _playerHealthSystem;
        
        private readonly List<HealthPointVisualizer> _healthPoints = new();

        private void Start()
        {
            Initialize();
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _playerHealthSystem.OnHealthChanged += OnHealthChanged;
        }
        private void RemoveListeners()
        {
            _playerHealthSystem.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int health)
        {
            for (var i = 0; i < health; i++)
            {
                _healthPoints[i].Activate();
            }
            
            for (var i = health; i < _healthPoints.Count; i++)
            {
                _healthPoints[i].Deactivate();
            }
        }

        private void Initialize()
        {
            _playerHealthSystem = PlayerController.Instance.PlayerHealthSystem;
            
            if (!_playerHealthSystem) return;
            
            InitializeHealthPoints();
        }

        private void InitializeHealthPoints()
        {
            if (!healthPointPrefab) return;
            if (!healthPointsContainer) return;
            
            for (var i = 0; i < _playerHealthSystem.MaxHealth; i++)
            {
                var healthPointInstance = Instantiate(healthPointPrefab, healthPointsContainer);
                
                _healthPoints.Add(healthPointInstance);
            }
            
            OnHealthChanged(_playerHealthSystem.CurrentHealth);
        }
    }
}