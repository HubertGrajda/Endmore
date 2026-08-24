namespace Scripts
{
    public interface ITimeService
    {
        public float TargetTimeScale { get; }
        public float TimeScaleModifier { get; }
        public void SetTargetTimeScale(float targetTimeScale);
        public void SetTimeScaleModifier(float modifier);
        
    }
}