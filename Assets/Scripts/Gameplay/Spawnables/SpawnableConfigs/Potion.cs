using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Potion : Spawnable, IInteractable
    {
        [SerializeField] private List<Effect> playerEffects;
        [SerializeField] private UnityEvent onCollected;
        
        public void Interact(GameObject interactor)
        {
            var applied = false;
            foreach (var effect in playerEffects)
            {
                if (effect.TryToApply(interactor))
                {
                    applied = true;
                }
            }
            
            if (!applied) return;
            
            onCollected?.Invoke();
            SpawnableFactory.ReturnToPool(this);
        }
    }
}