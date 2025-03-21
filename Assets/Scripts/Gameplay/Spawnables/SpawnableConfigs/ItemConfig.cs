using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Spawnable/Item")]
    public class ItemConfig : TileBasedSpawnableConfig<Item>
    {
    }
}