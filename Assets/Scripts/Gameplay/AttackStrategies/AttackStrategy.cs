using UnityEngine;

namespace Scripts.Gameplay
{
    public abstract class AttackStrategy : ScriptableObject
    {
        public abstract void ExecuteAttack(Enemy enemy);
    }
}