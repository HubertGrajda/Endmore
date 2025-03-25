using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "SpellAttack", menuName = "ScriptableObjects/AttackStrategy/SpellAttack")]
    public class SpellAttack : AttackStrategy
    {
        [SerializeField] private SpellConfig spellConfig;
        
        public override void ExecuteAttack(Enemy enemy)
        {
            enemy.StartCoroutine(AttackCoroutine(enemy));
        }

        private IEnumerator AttackCoroutine(Enemy enemy)
        {
            var spell = (Spell)SpawnableFactory.SpawnFromPool(spellConfig);
            //TODO: Spell placement
            spell.transform.position = enemy.transform.position + Vector3.right *1;
            
            spell.CastSpell();
            yield return new WaitForSeconds(spellConfig.CastingTime);
            spell.LaunchSpell();
            yield return new WaitForSeconds(spellConfig.LaunchingDuration);
            spell.Clear();
        }
    }
}