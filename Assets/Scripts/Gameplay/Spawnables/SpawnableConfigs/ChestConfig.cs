using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "ChestConfig", menuName = "ScriptableObjects/Spawnable/ChestConfig")]
    public class ChestConfig : SpawnableConfig
    {
        [field: SerializeField] public Sprite ChestOpenedSprite { get; private set; }
        [field: SerializeField] public List<SpawnableConfig> Content { get; private set; }
    }
}