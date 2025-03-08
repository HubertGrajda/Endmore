using UnityEngine;

namespace Scripts.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerInteractionLauncher : InteractionLauncher
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Interactable.Interact(other.gameObject);
        }
    }
}