using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts.Gameplay
{
    public class SpawnableFactory : Singleton<SpawnableFactory>
    {
        private readonly Dictionary<SpawnableConfig, IObjectPool<Spawnable>> _pools = new();

        public static Spawnable SpawnFromPool(SpawnableConfig config) => Instance.GetOrCreatePool(config).Get();

        public static void ReturnToPool(Spawnable spawnable)
        {
            if (!spawnable.gameObject.activeInHierarchy) return;
            
            Instance.GetOrCreatePool(spawnable.Config).Release(spawnable);
        }
        
        private IObjectPool<Spawnable> GetOrCreatePool(SpawnableConfig config)
        {
            if (TryGetPool(config, out var pool)) return pool;

            var container = new GameObject($"{config.name}_Pool");
            container.transform.SetParent(transform);
            
            pool = new ObjectPool<Spawnable>(
                () => config.Create(container.transform),
                config.OnGet,
                config.OnRelease,
                config.OnDestruction);
            
            _pools.Add(config, pool);
            return pool;
        }
        
        private bool TryGetPool(SpawnableConfig config, out IObjectPool<Spawnable> pool) =>
            _pools.TryGetValue(config, out pool);
    }
}