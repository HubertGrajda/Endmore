using UnityEngine;

namespace Scripts.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerInteractionLauncher : InteractionLauncher
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            foreach (var interactable in Interactables)
            {
                interactable.Interact(other.gameObject);
            }
        }
    }
}