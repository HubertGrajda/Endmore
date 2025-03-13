using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class Potion : Spawnable, IInteractable
    {
        [SerializeField] private List<Effect> playerEffects;
        
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
            
            SpawnableFactory.ReturnToPool(this);
        }
    }
}