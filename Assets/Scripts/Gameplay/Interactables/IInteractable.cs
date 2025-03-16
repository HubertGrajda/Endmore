using UnityEngine;

namespace Scripts.Gameplay
{
    public interface IInteractable
    {
        bool CanInteract => true;
        
        void Interact(GameObject interactor);
    }
}