using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : Spawnable<ProjectileConfig>, IInteractable
    {
        private Rigidbody2D _rigidbody;
        private ProjectileLauncher _launcher;
        private bool _hit;
        
        public override void Initialize(SpawnableConfig config)
        {
            base.Initialize(config);
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            StopAllCoroutines();
            _hit = false;
            _launcher = null;
        }

        public void Launch(Vector3 direction, ProjectileLauncher launcher)
        {
            _launcher = launcher;
            transform.position = launcher.transform.position;
            StartCoroutine(MovementCoroutine(direction, Config.Speed));
        }

        private IEnumerator MovementCoroutine(Vector2 direction, float speed)
        {
            while (!_hit)
            {
                _rigidbody.MovePosition(_rigidbody.position + direction * (speed * Time.fixedDeltaTime));
                yield return new WaitForFixedUpdate();
            }
        }
        
        public void Interact(GameObject interactor)
        {
            if (_hit || (_launcher && interactor == _launcher.gameObject)) return;
            
            _hit = true;
            ApplyDamage(interactor);
            ApplyKnockback(interactor);
            Clear();
        }
        
        private void ApplyKnockback(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out IKnockbackable knockbackable)) return;
            
            var direction = (interactor.transform.position - transform.position).normalized;
            knockbackable.ApplyKnockback(direction * Config.KnockBackStrength, Config.KnockBackDuration);
        }

        private void ApplyDamage(GameObject interactor)
        {
            if (!interactor.TryGetComponent(out IDamagable damagable)) return;
            
            damagable.TakeDamage(Config.Damage);
        }
    }
}