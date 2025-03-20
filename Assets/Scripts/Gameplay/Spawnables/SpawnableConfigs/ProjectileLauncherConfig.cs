using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "ProjectileLauncher", menuName = "ScriptableObjects/Spawnable/ProjectileLauncher")]
    public class ProjectileLauncherConfig : TileBasedSpawnableConfig<ProjectileLauncher>
    {
        [field: Header("Projectile Launcher Settings")]
        [field: SerializeField] public float LaunchingCooldown { get; private set; }
        [field: SerializeField] public ProjectileConfig ProjectileConfig { get; private set; }
        [field: SerializeField] public DirectionsSet DirectionsSet { get; private set; }
    }
}