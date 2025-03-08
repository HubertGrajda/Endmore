using UnityEngine;

namespace Scripts.Gameplay
{
    public interface IInteractable
    {
        void Interact(GameObject interactor);
    }
}