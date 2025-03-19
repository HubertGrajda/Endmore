using System;
using Newtonsoft.Json;

namespace Scripts.Gameplay
{
    [Serializable]
    public class LevelAttemptData : IComparable<LevelAttemptData>
    {
        [JsonProperty] private int _levelNumber;
        [JsonProperty] private string _playerName;
        [JsonProperty] private int _collisions;
        [JsonProperty] private double _secondsOfGameplay;
        
        public LevelAttemptData(int levelNumber, string playerName, int collisions, TimeSpan time)
        {
            _levelNumber = levelNumber;
            _playerName = playerName;
            _collisions = collisions;
            _secondsOfGameplay = time.TotalSeconds;
        }
        
        public void GetData(out int levelNumber, out string playerName, out int collisions, out TimeSpan time)
        {
            levelNumber = _levelNumber;
            playerName = _playerName;
            collisions = _collisions;
            time = GetTime();
        }
        
        private TimeSpan GetTime() => TimeSpan.FromSeconds(_secondsOfGameplay);
        
        public int CompareTo(LevelAttemptData otherAttempt)
        {
            var otherAttemptTime = otherAttempt.GetTime();
            var thisAttemptTime = GetTime();
            
            if (_levelNumber < otherAttempt._levelNumber) return 1;
            if (_levelNumber > otherAttempt._levelNumber) return -1;
            
            if (otherAttemptTime < thisAttemptTime) return 1;
            if (otherAttemptTime > thisAttemptTime) return -1;
            
            return 0;
        }
    }
}