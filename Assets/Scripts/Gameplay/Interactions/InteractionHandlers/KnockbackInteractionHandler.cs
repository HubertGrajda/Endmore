using UnityEngine;

namespace Scripts.Gameplay
{
    public class KnockbackInteractionHandler : MonoBehaviour, IInteractable
    {
        private IKnockBackProvider _knockbackProvider;

        private float _strength;
        private float _duration;
        
        public bool CanInteract { get; private set; }
        
        private void Start()
        {
            CanInteract = TryGetComponent(out _knockbackProvider);

            if (!CanInteract) return;
            
            _strength = _knockbackProvider.KnockbackStrength;
            _duration = _knockbackProvider.KnockbackDuration;
        }
        
        public void Interact(GameObject interactor)
        {
            ApplyKnockback(interactor);
        }
        
        private void ApplyKnockback(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out IKnockbackable knockbackable)) return;
            
            var direction = (interactor.transform.position - transform.position).normalized;
            knockbackable.ApplyKnockback(direction * _strength, _duration);
        }
    }
}