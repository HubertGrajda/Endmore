using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "Chest", menuName = "ScriptableObjects/Spawnable/Chest")]
    public class ChestConfig : TileBasedSpawnableConfig<Chest>
    {
        [field: Header("Chest Settings")]
        [field: SerializeField] public Sprite ChestOpenedSprite { get; private set; }
        [field: SerializeField] public List<SpawnableConfig> Content { get; private set; }
        [field: SerializeField] public ItemConfig KeyItem { get; private set; }
    }
}