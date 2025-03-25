using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Gameplay
{
    public abstract class Spawnable : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

        protected GameplayManager GameplayManager { get; private set; }

        public virtual void Initialize(SpawnableConfig config)
        {
            transform.localScale *= config.ScaleFactor;
            
            if (SpriteRenderer && config.Sprite)
            {
                SpriteRenderer.sprite = config.Sprite;
                SpriteRenderer.color = config.Color;
            }

            GameplayManager = GameplayManager.Instance;
        }

        public virtual void OnSpawn()
        {
            GameplayManager.OnLevelClear += Clear;
        }
        
        public virtual void OnDespawn()
        {
            GameplayManager.OnLevelClear -= Clear;
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