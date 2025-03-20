using UnityEngine;

namespace Scripts.Gameplay
{
    public abstract class Spawnable : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;
        
        public virtual void Initialize(SpawnableConfig config)
        {
            transform.localScale *= config.ScaleFactor;
            
            if (spriteRenderer && config.Sprite)
            {
                spriteRenderer.sprite = config.Sprite;
                spriteRenderer.color = config.Color;
            }
        }

        public virtual void OnSpawn()
        {
        }
        
        public virtual void OnDespawn()
        {
        }
        
        public abstract void Clear();
    }

    public class Spawnable<TConfigType> : Spawnable where TConfigType : SpawnableConfig
    {
        public TConfigType Config { get; private set; }

        public override void Initialize(SpawnableConfig config)
        {
            base.Initialize(config);
            Config = (TConfigType)config;
        }

        public override void Clear()
        {
            SpawnableFactory.ReturnToPool(this);
        }
    }
}