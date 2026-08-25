using Reflex.Attributes;
using Scripts.UI;
using UnityEngine;

namespace Scripts
{
    public class LoadSceneButton : ActionButton
    {
        [SerializeField] private string sceneName;
        
        [Inject] private IScenesService _scenesService;

        protected override bool IsValid => _scenesService != null && !string.IsNullOrWhiteSpace(sceneName);

        protected override void OnClick()
        {
            _scenesService.LaunchSceneByName(sceneName);
        }
    }
}