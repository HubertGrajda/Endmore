using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Coin : Spawnable<CoinConfig>, IInteractable
    {
        [SerializeField] private UnityEvent onCollected;

        [Inject] private IScoreService _scoreService;

        public override void OnSpawn()
        {
            base.OnSpawn();
            _scoreService.IncreaseTotalPointsBy(Config.CoinValue);
        }

        private void Collect()
        {
            Clear();
            _scoreService.AddScore(Config.CoinValue);
            onCollected?.Invoke();
        }

        public void Interact(GameObject interactor) => Collect();
    }
}