using UnityEngine;

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
            var gameplayManager = GameplayManager.Instance;

            if (!gameplayManager) return false;
            
            return type switch
            {
                Type.Equal => gameplayManager.CurrentLevel == level,
                Type.Above => gameplayManager.CurrentLevel > level,
                Type.Below => gameplayManager.CurrentLevel < level,
                _ => true
            };
        }
    }
}