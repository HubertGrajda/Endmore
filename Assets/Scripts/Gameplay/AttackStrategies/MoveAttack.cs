using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "MoveAttack", menuName = "ScriptableObjects/AttackStrategy/MoveAttack")]
    public class MoveAttack : AttackStrategy
    {
        [SerializeField] private DirectionsSet directionsSet;
        [SerializeField] private int range;
        
        public override void ExecuteAttack(Enemy enemy)
        {
            enemy.StartCoroutine(AttackCoroutine(enemy));
        }

        private Vector3 GetPosition(Enemy enemy) => 
            enemy.transform.position + range * (Vector3)directionsSet.GetRandomVector();
        
        private IEnumerator AttackCoroutine(Enemy launcher)
        {
            var oneWayTime = launcher.Config.AttackCooldown / 2;
            var currentPosition = launcher.transform.position;
            var position = GetPosition(launcher);
            
            yield return launcher.StartCoroutine(MoveCoroutine(launcher, oneWayTime, position));
            
            launcher.StartCoroutine(MoveCoroutine(launcher, oneWayTime, currentPosition));
        }

        private IEnumerator MoveCoroutine(Enemy launcher, float duration, Vector3 position)
        {
            var timer = 0f;
            var startPosition = launcher.transform.position;
            launcher.SpriteRenderer.flipX = startPosition.x - position.x > 0;
            
            while (timer <= duration)
            {
                launcher.transform.position = Vector3.Lerp(startPosition, position, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}