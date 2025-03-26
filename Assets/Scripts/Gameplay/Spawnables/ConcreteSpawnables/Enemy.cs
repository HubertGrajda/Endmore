using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class Enemy : Spawnable<EnemyConfig>, IDamageProvider
    {
        [SerializeField] private Animator animator;
        
        private bool _isActive;
        
        private static readonly int LaunchAttackAnimation = Animator.StringToHash("Launch");
        
        public int DamageAmount => Config.ContactDamage;
        
        public override void OnSpawn()
        {
            base.OnSpawn();
            GameplayManager.OnLevelStarted += OnLevelStarted;
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            
            StopAllCoroutines();
            GameplayManager.OnLevelStarted -= OnLevelStarted;
        }

        private void OnLevelStarted(int _)
        {
            StartCoroutine(AttackingCoroutine());
        }

        private IEnumerator AttackingCoroutine()
        {
            _isActive = true;
            
            while (_isActive)
            {
                yield return new WaitForSeconds(Config.AttackCooldown);

                yield return AttackAnimationCoroutine();
                
                Config.AttackStrategy.ExecuteAttack(this);
            }
        }

        private IEnumerator AttackAnimationCoroutine()
        {
            if (animator == null) yield break;
            
            animator.SetTrigger(LaunchAttackAnimation);
                    
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            yield return null;
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
}