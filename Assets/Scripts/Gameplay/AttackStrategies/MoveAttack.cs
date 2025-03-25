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

        private Vector3 GetPosition(Enemy launcher)
        {
            var vectors = directionsSet.GetVectors();
            var drawnDirection = (Vector3)vectors[Random.Range(0, vectors.Count)];
            return launcher.transform.position - range * drawnDirection;
        }
        
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
            
            while (timer <= duration)
            {
                launcher.transform.position = Vector3.Lerp(startPosition, position, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}