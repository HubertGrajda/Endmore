using UnityEngine;

namespace Scripts.Gameplay
{
    public interface IKnockbackable
    {
        void ApplyKnockback(Vector2 force, float duration);
    }
}