using Reflex.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "RequiredLevelCondition", menuName = "ScriptableObjects/GameStateCondition/RequiredLevel")]
    public class RequiredLevelCondition : GameStateCondition
    {
        [SerializeField] private Type type;
        [SerializeField] private int level;
        
        private enum Type
        {
            Equal,
            Above,
            Below
        }

        public override bool Met()
        {
            var container = SceneManager.GetActiveScene().GetSceneContainer();
            var gameplayService = container.Resolve<IGameplayService>();
            
            return type switch
            {
                Type.Equal => gameplayService.CurrentLevel == level,
                Type.Above => gameplayService.CurrentLevel > level,
                Type.Below => gameplayService.CurrentLevel < level,
                _ => true
            };
        }
    }
}