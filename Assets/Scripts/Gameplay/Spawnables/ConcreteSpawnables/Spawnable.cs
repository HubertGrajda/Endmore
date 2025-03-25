using UnityEngine;

namespace Scripts.Gameplay
{
    public abstract class Spawnable : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;
        
        private GameplayManager _gameplayManager;
        
        public virtual void Initialize(SpawnableConfig config)
        {
            transform.localScale *= config.ScaleFactor;
            
            if (spriteRenderer && config.Sprite)
            {
                spriteRenderer.sprite = config.Sprite;
                spriteRenderer.color = config.Color;
            }

            _gameplayManager = GameplayManager.Instance;
        }

        public virtual void OnSpawn()
        {
            _gameplayManager.OnLevelClear += Clear;
        }
        
        public virtual void OnDespawn()
        {
            _gameplayManager.OnLevelClear -= Clear;
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