using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Coin : Spawnable, IInteractable
    {
        [SerializeField] private UnityEvent onCollected;

        private CoinConfig _config;

        public override void Initialize(SpawnableConfig config)
        {
            base.Initialize(config);
            
            if (config is not CoinConfig coinConfig) return;
            
            _config = coinConfig;
        }

        private void Collect()
        {
            SpawnableFactory.ReturnToPool(this);
            ScoreManager.Instance.AddScore(_config.CoinValue);
            onCollected?.Invoke();
        }

        public void Interact(GameObject interactor) => Collect();
    }
}