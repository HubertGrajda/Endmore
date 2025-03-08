using UnityEngine;

namespace Scripts.Gameplay
{
    public class Spawnable : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public SpawnableConfig Config { get; private set; }

        public virtual void Initialize(SpawnableConfig config)
        {
            Config = config;

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
    }
}