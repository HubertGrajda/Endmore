using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Spell : Spawnable<SpellConfig>, IDamageProvider
    {
        [SerializeField] private SpriteRenderer castingSprite;
        
        [SerializeField] private UnityEvent onDespawn;
        [SerializeField] private UnityEvent onCast;
        [SerializeField] private UnityEvent onLaunch;

        public int DamageAmount => Config.Damage;

        public override void Initialize(SpawnableConfig config)
        {
            base.Initialize(config);

            if (castingSprite)
            {
                castingSprite.sprite = Config.CastingSprite;
            }
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            onDespawn?.Invoke();
        }

        public void CastSpell()
        {
            onCast?.Invoke();
        }

        public void LaunchSpell()
        {
            onLaunch?.Invoke();
        }
    }
}