using Reflex.Attributes;
using Scripts.UI;
using UnityEngine;

namespace Scripts
{
    public class LoadSceneButton : ActionButton
    {
        [SerializeField] private string sceneName;
        
        [Inject] private IScenesService _scenesManager;

        protected override bool IsValid => _scenesManager != null && !string.IsNullOrWhiteSpace(sceneName);

        protected override void OnClick()
        {
            _scenesManager.LaunchSceneByName(sceneName);
        }
    }
}