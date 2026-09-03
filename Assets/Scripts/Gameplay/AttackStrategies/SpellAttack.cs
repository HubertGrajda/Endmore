using System.Collections;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "SpellAttack", menuName = "ScriptableObjects/AttackStrategy/SpellAttack")]
    public class SpellAttack : AttackStrategy
    {
        [SerializeField] private SpellConfig spellConfig;
        [SerializeField] private DirectionsSet directionsSet;
        [SerializeField] private float maxDistance;
        
        public override void ExecuteAttack(Enemy enemy, GameObject target)
        {
            enemy.StartCoroutine(AttackCoroutine(enemy, target));
        }

        private IEnumerator AttackCoroutine(Enemy enemy, GameObject target)
        {
            var spell = enemy.SpawnSpell(spellConfig);
            var targetPosition = target.transform.position;
            var enemyPosition = enemy.transform.position;

            spell.transform.position = Vector3.Distance(enemyPosition, targetPosition) > maxDistance 
                ? enemyPosition + (Vector3)directionsSet.GetRandomVector() 
                : targetPosition;
            
            spell.CastSpell();
            yield return new WaitForSeconds(spellConfig.CastingTime);
            spell.LaunchSpell();
            yield return new WaitForSeconds(spellConfig.LaunchingDuration);
            spell.Clear();
        }
    }
}