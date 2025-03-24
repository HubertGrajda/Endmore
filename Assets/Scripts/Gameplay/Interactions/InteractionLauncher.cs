using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Gameplay
{
    [RequireComponent(typeof(IInteractable))]
    public abstract class InteractionLauncher : MonoBehaviour
    {
        private IInteractable[] _interactables;
        
        protected List<IInteractable> Interactables => _interactables.Where(x => x.CanInteract).ToList();
        
        private void Awake()
        {
            _interactables = GetComponents<IInteractable>();
        }
    }
}