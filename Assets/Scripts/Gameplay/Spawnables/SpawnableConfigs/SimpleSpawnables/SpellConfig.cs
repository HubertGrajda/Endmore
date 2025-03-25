using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/Spawnable/Spell")]
    public class SpellConfig : SpawnableConfig<Spell>
    {
        [field: SerializeField] public Sprite CastingSprite { get; private set; }
        [field: SerializeField] public float CastingTime { get; private set; }
        [field: SerializeField] public float LaunchingDuration { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
    }
}