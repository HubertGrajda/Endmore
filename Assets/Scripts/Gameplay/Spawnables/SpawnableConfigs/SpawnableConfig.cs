using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class SpawnableConfig : ScriptableObject
    {
        [field: SerializeField] public List<PlacementCondition> PlacementConditions { get; set; }
        [field: SerializeField] public List<GameStateCondition> GameStateConditions { get; set; }
        [field: SerializeField] public Spawnable Prefab { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; set; }
        [field: SerializeField] public Color Color { get; set; } = Color.white;
        [field: SerializeField] public float ScaleFactor { get; set; } = 1f;

        public Spawnable Create(Transform transform)
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

        public virtual void OnGet(Spawnable spawnable)
        {
            spawnable.gameObject.SetActive(true);
            spawnable.OnSpawn();
        }

        public void OnRelease(Spawnable spawnable)
        {
            spawnable.OnDespawn();
            spawnable.gameObject.SetActive(false);
        }

        public void OnDestruction(Spawnable spawnable) => Destroy(spawnable.gameObject);
    }
}