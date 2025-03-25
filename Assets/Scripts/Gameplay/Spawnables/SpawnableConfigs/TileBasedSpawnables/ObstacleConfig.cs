using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "Obstacle", menuName = "ScriptableObjects/Spawnable/Obstacle")]
    public class ObstacleConfig : TileBasedSpawnableConfig<Obstacle>
    {
    }
}