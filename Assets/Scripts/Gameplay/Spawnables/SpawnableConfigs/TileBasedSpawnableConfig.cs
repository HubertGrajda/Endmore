using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    public abstract class TileBasedSpawnableConfig : SpawnableConfig
    {
        [field: SerializeField] public List<PlacementCondition> PlacementConditions { get; set; }
        [field: SerializeField] public List<GameStateCondition> GameStateConditions { get; set; }
    }
    
    public abstract class TileBasedSpawnableConfig<TSpawnable> : TileBasedSpawnableConfig where TSpawnable : Spawnable
    {
        [field: SerializeField] public TSpawnable Prefab { get; private set; }
        
        public override Spawnable Create(Transform transform)
        {
            if (Prefab == null)
            {
                Debug.LogError($"{name}: has unassigned {nameof(Prefab)}. Object will not be spawned.");
                return null;
            }
            
            var spawnableInstance = Instantiate(Prefab, transform);
            
            spawnableInstance.gameObject.SetActive(false);
            spawnableInstance.name = Prefab.name;
            spawnableInstance.Initialize(this);

            return spawnableInstance;
        }
    }
}