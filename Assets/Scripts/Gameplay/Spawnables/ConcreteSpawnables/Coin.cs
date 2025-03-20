using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Coin : Spawnable<CoinConfig>, IInteractable
    {
        [SerializeField] private UnityEvent onCollected;

        private void Collect()
        {
            Clear();
            ScoreManager.Instance.AddScore(Config.CoinValue);
            onCollected?.Invoke();
        }

        public void Interact(GameObject interactor) => Collect();
    }
}