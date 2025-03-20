using UnityEngine;

namespace Scripts.Gameplay
{
    public abstract class SpawnableConfig : ScriptableObject
    {
        [Header("Spawnable Settings")]
        [field: SerializeField] public Sprite Sprite { get; set; }
        [field: SerializeField] public Color Color { get; set; } = Color.white;
        [field: SerializeField] public float ScaleFactor { get; set; } = 1f;

        public abstract Spawnable Create(Transform transform);

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

    public abstract class SpawnableConfig<TSpawnable> : SpawnableConfig where TSpawnable : Spawnable
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