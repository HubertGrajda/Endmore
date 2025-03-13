using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "SpeedUpEffect", menuName = "ScriptableObjects/Effect/SpeedUp")]
    public class SpeedUpEffect : Effect
    {
        [SerializeField] private float duration;
        [SerializeField] private int speed;
        
        public override bool TryToApply(GameObject target)
        {
            if (!target.TryGetComponent(out IMovable movable)) return false;
            
            movable.SpeedUp(speed, duration);
            return true;
        }
    }
}