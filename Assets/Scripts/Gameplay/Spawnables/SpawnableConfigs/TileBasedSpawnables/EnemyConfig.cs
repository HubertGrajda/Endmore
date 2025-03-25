using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Spawnable/Enemy")]
    public class EnemyConfig : TileBasedSpawnableConfig<Enemy>
    {
        [field: Header("Enemy Attack Settings")]
        [field: SerializeField] public float AttackCooldown { get; private set; }
        
        [field: SerializeField] public AttackStrategy AttackStrategy { get; private set; }
    }
}