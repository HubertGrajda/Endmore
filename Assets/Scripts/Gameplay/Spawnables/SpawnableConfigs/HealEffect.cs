using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "HealEffect", menuName = "ScriptableObjects/Effect/Heal")]
    public class HealEffect : Effect
    {
        [SerializeField] private int healAmount;
        
        public override bool TryToApply(GameObject target)
        {
            if (!target.TryGetComponent(out IHealable healable)) return false;
            
            healable.Heal(healAmount);
            return true;
        }
    }
}