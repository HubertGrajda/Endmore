using Reflex.Attributes;
using UnityEngine;

namespace Scripts.Gameplay
{
    public abstract class Spawnable : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

        [Inject] protected IGameplayService GameplayService { get; private set; }

        public virtual void Initialize(SpawnableConfig config)
        {
            transform.localScale *= config.ScaleFactor;
            
            if (SpriteRenderer && config.Sprite)
            {
                SpriteRenderer.sprite = config.Sprite;
                SpriteRenderer.color = config.Color;
            }
        }

        public virtual void OnSpawn()
        {
            GameplayService.OnLevelClear += Clear;
        }
        
        public virtual void OnDespawn()
        {
            GameplayService.OnLevelClear -= Clear;
        }
        
        public abstract void Clear();
    }

    public class Spawnable<TConfigType> : Spawnable where TConfigType : SpawnableConfig
    {
        public TConfigType Config { get; private set; }

        [Inject] private SpawnableFactory _factory;
        
        protected SpawnableFactory Factory => _factory;
        
        public override void Initialize(SpawnableConfig config)
        {
            base.Initialize(config);
            Config = (TConfigType)config;
        }

        public override void Clear()
        {
            _factory.ReturnToPool(this);
        }
    }
}