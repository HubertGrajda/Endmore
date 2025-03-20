using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Spawnable/Projectile")]
    public class ProjectileConfig : SpawnableConfig<Projectile>
    {
        [field: Header("Projectile Settings")]
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float KnockBackStrength { get; private set; }
        [field: SerializeField] public float KnockBackDuration { get; private set; }
    }
}