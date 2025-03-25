using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : Spawnable<ProjectileConfig>, IInteractable, IDamageProvider, IKnockBackProvider
    {
        private Rigidbody2D _rigidbody;
        private Enemy _enemy;
        private bool _hit;
        
        public int DamageAmount => Config.Damage;
        public float KnockbackStrength => Config.KnockBackStrength;
        public float KnockbackDuration => Config.KnockBackDuration;
        
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
            _enemy = null;
        }

        public void Launch(Vector3 direction, Enemy enemy)
        {
            _enemy = enemy;
            transform.position = enemy.transform.position;
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
            if (_hit || (_enemy && interactor == _enemy.gameObject)) return;
            
            _hit = true;
            Clear();
        }
    }
}