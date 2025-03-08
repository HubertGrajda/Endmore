using UnityEngine;

namespace Scripts.Gameplay
{
    public abstract class GameStateCondition : ScriptableObject
    {
        public abstract bool Met();
    }
}