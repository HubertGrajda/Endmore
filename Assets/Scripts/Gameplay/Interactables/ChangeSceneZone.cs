using Reflex.Attributes;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class ChangeSceneZone : MonoBehaviour, IInteractable
    {
        [SerializeField] private string sceneName;
        
        [Inject] private IScenesService _scenesManager;

        public void Interact(GameObject interactor)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            
            _scenesManager.LaunchSceneByName(sceneName);
        }
    }
}