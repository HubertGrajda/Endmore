using UnityEngine;

namespace Scripts.Gameplay
{
    public class ChangeSceneZone : MonoBehaviour, IInteractable
    {
        [SerializeField] private string sceneName;
        
        private ScenesManager _scenesManager;

        private void Awake()
        {
            _scenesManager = ScenesManager.Instance;
        }

        public void Interact(GameObject interactor)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            
            _scenesManager.LaunchSceneByName(sceneName);
        }
    }
}