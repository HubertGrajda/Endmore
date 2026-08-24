using System.Collections.Generic;
using Scripts.Gameplay;

namespace Scripts
{
    public interface IGameService
    {
        public string PlayerName { get; }
        public List<LevelAttemptData> LevelToAttemptsData { get; }
        
        public void AddAttempt(LevelAttemptData attemptData);
        public void ChangePlayerName(string value);
    }
}