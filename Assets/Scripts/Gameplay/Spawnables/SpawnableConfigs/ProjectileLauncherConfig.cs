using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "ProjectileLauncher", menuName = "ScriptableObjects/Spawnable/ProjectileLauncher")]
    public class ProjectileLauncherConfig : SpawnableConfig
    {
        [field: SerializeField] public ProjectileConfig ProjectileConfig { get; private set; }
        [field: SerializeField] public float LaunchingCooldown { get; private set; }
        [field: SerializeField] public ShootingDirection Direction { get; private set; }
    }
}