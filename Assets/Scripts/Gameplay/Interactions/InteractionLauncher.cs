using UnityEngine;

namespace Scripts.Gameplay
{
    [RequireComponent(typeof(IInteractable))]
    public abstract class InteractionLauncher : MonoBehaviour
    {
        protected IInteractable Interactable { get; private set; }

        private void Awake()
        {
            Interactable = GetComponent<IInteractable>();
        }
    }
}