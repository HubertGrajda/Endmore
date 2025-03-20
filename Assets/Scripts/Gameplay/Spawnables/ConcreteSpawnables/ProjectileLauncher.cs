using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class ProjectileLauncher : Spawnable
    {
        [SerializeField] private Animator animator;
        
        private ProjectileLauncherConfig _config; 
        private GameplayManager _gameplayManager; 
        private bool _isActive;
        
        private static readonly int Launch = Animator.StringToHash("Launch");
        
        public override void Initialize(SpawnableConfig config)
        {
            base.Initialize(config);
            
            if (config is not ProjectileLauncherConfig obstacleConfig) return;
            
            _config = obstacleConfig;
            _gameplayManager = GameplayManager.Instance;
        }
        
        public override void OnSpawn()
        {
            base.OnSpawn();
            _gameplayManager.OnLevelStarted += OnLevelStarted;
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            
            StopAllCoroutines();
            _gameplayManager.OnLevelStarted -= OnLevelStarted;
        }

        private void OnLevelStarted(int _)
        {
            StartCoroutine(ShootingCoroutine());
        }

        private IEnumerator ShootingCoroutine()
        {
            _isActive = true;
            
            while (_isActive)
            {
                yield return new WaitForSeconds(_config.LaunchingCooldown);

                yield return LaunchingAnimation();
                
                if (_config.DirectionsSet == null) yield break;
                
                var directionsToShoot = _config.DirectionsSet.GetVectors();

                foreach (var direction in directionsToShoot)
                {
                    var projectile = (Projectile)SpawnableFactory.SpawnFromPool(_config.ProjectileConfig);
                    
                    projectile.Launch(direction, this);
                }
            }
        }

        private IEnumerator LaunchingAnimation()
        {
            if (animator == null) yield break;
            
            animator.SetTrigger(Launch);
                    
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            yield return null;
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
}