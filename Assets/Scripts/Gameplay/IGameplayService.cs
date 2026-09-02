using System;

namespace Scripts.Gameplay
{
    public interface IGameplayService
    {
        event Action OnLevelClear;
        event Action<int> OnLevelStarted;
        event Action<int> OnCollisionsNumberChanged;
        
        int CurrentLevel { get; }
        int CollisionsNumber { get; }
        Timer GameplayTimer { get; }
        
        void IncrementCollisionsNumber();
        void FinishAndRestart();
    }
}