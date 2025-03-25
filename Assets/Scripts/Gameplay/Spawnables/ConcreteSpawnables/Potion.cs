using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Potion : Spawnable<PotionConfig>, IInteractable
    {
        [SerializeField] private UnityEvent onCollected;
        
        public void Interact(GameObject interactor)
        {
            var applied = false;
            
            foreach (var effect in Config.Effects)
            {
                if (effect.TryToApply(interactor))
                {
                    applied = true;
                }
            }
            
            if (!applied) return;
            
            onCollected?.Invoke();
            Clear();
        }
    }
}