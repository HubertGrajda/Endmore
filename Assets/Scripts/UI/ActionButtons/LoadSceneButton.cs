using Scripts.UI;
using UnityEngine;

namespace Scripts
{
    public class LoadSceneButton : ActionButton
    {
        [SerializeField] private string sceneName;
        
        private ScenesManager _scenesManager;

        protected override bool IsValid => _scenesManager != null && !string.IsNullOrWhiteSpace(sceneName);

        protected override void Prepare()
        {
            _scenesManager = ScenesManager.Instance;
        }

        protected override void OnClick()
        {
            _scenesManager.LaunchSceneByName(sceneName);
        }
    }
}