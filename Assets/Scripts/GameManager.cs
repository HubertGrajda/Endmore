using System.Collections.Generic;
using Newtonsoft.Json;
using Scripts.Gameplay;
using Scripts.SaveSystem;
using UnityEngine;

namespace Scripts
{
    public class GameManager : Singleton<GameManager>, ISaveable<GameData>
    {
        public Dictionary<int, List<LevelAttemptData>> LevelToAttemptsData { get; private set; } = new();

        public int GetAttemptNumber(int levelNumber) =>
            LevelToAttemptsData.TryGetValue(levelNumber, out var attemptsData) 
                ? attemptsData.Count 
                : 0;
        
        public void AddAttempt(int levelNumber, LevelAttemptData attemptData)
        {
            if (LevelToAttemptsData.TryAdd(levelNumber, new List<LevelAttemptData>{attemptData})) return;
                
            LevelToAttemptsData[levelNumber].Add(attemptData);
        }

        public SaveData Save() => new GameData(LevelToAttemptsData);

        public void Load(GameData data)
        {
            if (!data.TryGetData(out var levelToAttemptsData)) return;
            
            LevelToAttemptsData = levelToAttemptsData;
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
        }
        
        public void ResumeGame()
        {
            Time.timeScale = 1f;
        }
    }
    
    public class GameData : SaveData
    {
        [JsonProperty] private Dictionary<int, List<LevelAttemptData>> _levelToAttemptsData;

        public GameData(Dictionary<int, List<LevelAttemptData>> levelToAttemptsData)
        {
            _levelToAttemptsData = levelToAttemptsData;
        }

        public bool TryGetData(out Dictionary<int, List<LevelAttemptData>> levelToAttemptsData)
        {
            levelToAttemptsData = _levelToAttemptsData;
            return levelToAttemptsData != null;
        }
    }
}