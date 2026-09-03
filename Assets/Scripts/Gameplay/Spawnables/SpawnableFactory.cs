using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Scripts.Gameplay
{
    public class SpawnableFactory : MonoService<SpawnableFactory>
    {
        private readonly Dictionary<SpawnableConfig, IObjectPool<Spawnable>> _pools = new();

        public Spawnable SpawnFromPool(SpawnableConfig config)
        {
            return GetOrCreatePool(config).Get();
        }
        
        public TSpawnable SpawnFromPool<TSpawnable>(SpawnableConfig<TSpawnable> config) where TSpawnable : Spawnable
        {
            return (TSpawnable) GetOrCreatePool(config).Get();
        }
        
        public void ReturnToPool<TConfig>(Spawnable<TConfig> spawnable) where TConfig : SpawnableConfig
        {
            if (!spawnable.gameObject.activeInHierarchy) return;
            
            GetOrCreatePool(spawnable.Config).Release(spawnable);
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