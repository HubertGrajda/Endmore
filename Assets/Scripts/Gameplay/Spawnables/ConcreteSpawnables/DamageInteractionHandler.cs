using UnityEngine;

namespace Scripts.Gameplay
{
    public class DamageInteractionHandler : MonoBehaviour, IInteractable
    {
        private int _damage;

        private IDamageProvider _damageProvider;
        
        public bool CanInteract { get; private set; }

        private void Start()
        {
            CanInteract = TryGetComponent(out _damageProvider);
        }

        public void Interact(GameObject interactor)
        {
            ApplyDamage(interactor);
        }
        
        private void ApplyDamage(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out IDamagable damagable)) return;
            
            damagable.TakeDamage(_damageProvider.DamageAmount);
        }
    }
}