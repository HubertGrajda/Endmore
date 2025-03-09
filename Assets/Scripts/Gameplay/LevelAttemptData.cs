using System;
using Newtonsoft.Json;

namespace Scripts.Gameplay
{
    [Serializable]
    public class LevelAttemptData : IComparable<LevelAttemptData>
    {
        [JsonProperty] private int _levelNumber;
        [JsonProperty] private int _attemptNumber;
        [JsonProperty] private int _collisions;
        [JsonProperty] private double _secondsOfGameplay;
        
        public LevelAttemptData(int levelNumber, int attemptNumber, int collisions, TimeSpan time)
        {
            _levelNumber = levelNumber;
            _attemptNumber = attemptNumber;
            _collisions = collisions;
            _secondsOfGameplay = time.TotalSeconds;
        }
        
        public void GetData(out int levelNumber, out int attemptNumber, out int collisions, out TimeSpan time)
        {
            levelNumber = _levelNumber;
            attemptNumber = _attemptNumber;
            collisions = _collisions;
            time = GetTime();
        }
        
        private TimeSpan GetTime() => TimeSpan.FromSeconds(_secondsOfGameplay);
        
        public int CompareTo(LevelAttemptData otherAttempt)
        {
            var otherAttemptTime = otherAttempt.GetTime();
            var thisAttemptTime = GetTime();
            
            if (otherAttemptTime < thisAttemptTime) return 1;
            if (otherAttemptTime > thisAttemptTime) return -1;
            
            return 0;
        }
    }
}