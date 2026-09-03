using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "ProjectileAttack", menuName = "ScriptableObjects/AttackStrategy/ProjectileAttack")]
    public class ProjectileAttack : AttackStrategy
    {
        [SerializeField] private ProjectileConfig projectileConfig;
        [SerializeField] private DirectionsSet directionsSet;

        public override void ExecuteAttack(Enemy enemy, GameObject target)
        {
            if (directionsSet == null) return;
                
            var directionsToShoot = directionsSet.GetVectors();

            foreach (var direction in directionsToShoot)
            {
                var projectile = enemy.SpawnProjectile(projectileConfig);
                    
                projectile.Launch(direction, enemy);
            }
        }
    }
}