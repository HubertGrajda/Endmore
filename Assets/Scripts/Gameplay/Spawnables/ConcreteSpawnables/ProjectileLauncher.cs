using System;
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
                
                var direction = GetDirectionForProjectile();
                var projectile = (Projectile)SpawnableFactory.SpawnFromPool(_config.ProjectileConfig);
                
                projectile.Launch(direction, this);
            }
        }

        private Vector2 GetDirectionForProjectile() => _config.Direction switch 
        {
            ShootingDirection.None => Vector2.zero,
            ShootingDirection.Left => Vector2.left,
            ShootingDirection.Right => Vector2.right,
            ShootingDirection.Down => Vector2.down,
            ShootingDirection.Up => Vector2.up,
            _ => throw new ArgumentOutOfRangeException()
        };

        private IEnumerator LaunchingAnimation()
        {
            if (animator == null) yield break;
            
            animator.SetTrigger(Launch);
                    
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            yield return null;
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    public enum ShootingDirection
    {
        None = 0,
        Left = 1,
        Right = 2,
        Down = 3,
        Up = 4,
    }
}