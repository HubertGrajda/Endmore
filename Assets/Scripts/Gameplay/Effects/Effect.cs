using UnityEngine;

namespace Scripts.Gameplay
{
    public abstract class Effect : ScriptableObject
    {
        public abstract bool TryToApply(GameObject target);
    }
}