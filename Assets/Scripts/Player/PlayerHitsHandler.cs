using Scripts.Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Player
{
    public class PlayerHitsHandler : MonoBehaviour, IDamagable
    {
        [SerializeField] private UnityEvent onDamageTaken;
        
        private GameplayManager _gameplayManager;

        private void Start()
        {
            _gameplayManager = GameplayManager.Instance;
        }

        public void TakeDamage()
        {
            onDamageTaken?.Invoke();
            _gameplayManager.IncrementCollisionsNumber();
        }
    }
}