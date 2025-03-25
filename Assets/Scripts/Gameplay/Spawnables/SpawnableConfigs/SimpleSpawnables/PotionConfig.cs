using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "Potion", menuName = "ScriptableObjects/Spawnable/Potion")]
    public class PotionConfig : SpawnableConfig<Potion>
    {
        [field: Header("Potion Settings")]
        [field: SerializeField] public List<Effect> Effects { get; private set; }
    }
}