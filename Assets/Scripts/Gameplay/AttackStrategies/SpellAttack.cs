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
        
        public override void ExecuteAttack(Enemy enemy)
        {
            enemy.StartCoroutine(AttackCoroutine(enemy));
        }

        private IEnumerator AttackCoroutine(Enemy enemy)
        {
            var spell = (Spell)SpawnableFactory.SpawnFromPool(spellConfig);
            var playerPosition = PlayerController.Instance.transform.position;
            var enemyPosition = enemy.transform.position;

            spell.transform.position = Vector3.Distance(enemyPosition, playerPosition) > maxDistance 
                ? enemyPosition + (Vector3)directionsSet.GetRandomVector() 
                : playerPosition;
            
            spell.CastSpell();
            yield return new WaitForSeconds(spellConfig.CastingTime);
            spell.LaunchSpell();
            yield return new WaitForSeconds(spellConfig.LaunchingDuration);
            spell.Clear();
        }
    }
}