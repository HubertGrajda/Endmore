namespace Scripts.Gameplay
{
    public interface IKnockBackProvider
    {
        public float KnockbackStrength { get; }
        public float KnockbackDuration { get; }
    }
}