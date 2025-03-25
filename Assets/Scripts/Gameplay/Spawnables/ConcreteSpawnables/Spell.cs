using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Gameplay
{
    public class Spell : Spawnable<SpellConfig>, IDamageProvider
    {
        [SerializeField] private UnityEvent onDespawn;
        [SerializeField] private UnityEvent onCast;
        [SerializeField] private UnityEvent onLaunch;

        public int DamageAmount => Config.Damage;
        
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