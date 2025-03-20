using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "PotionConfig", menuName = "ScriptableObjects/Spawnable/PotionConfig")]
    public class PotionConfig : SpawnableConfig<Potion>
    {
        [field: Header("Potion Settings")]
        [field: SerializeField] public List<Effect> Effects { get; private set; }
    }
}