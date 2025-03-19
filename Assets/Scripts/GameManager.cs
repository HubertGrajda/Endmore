using System.Collections.Generic;
using Newtonsoft.Json;
using Scripts.Gameplay;
using Scripts.SaveSystem;
using UnityEngine;

namespace Scripts
{
    public class GameManager : Singleton<GameManager>, ISaveable<GameData>
    {
        public List<LevelAttemptData> LevelToAttemptsData { get; private set; } = new();
        public string PlayerName { get; private set; }

        public void ChangePlayerName(string newName)
        {
            PlayerName = newName;
        }
        
        public void AddAttempt(LevelAttemptData attemptData)
        {
            LevelToAttemptsData.Add(attemptData);
        }

        public SaveData Save() => new GameData(LevelToAttemptsData, PlayerName);

        public void Load(GameData data)
        {
            if (!data.TryGetData(out var levelToAttemptsData, out var playerName)) return;
            
            LevelToAttemptsData = levelToAttemptsData;
            PlayerName = playerName;
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
        [JsonProperty] private List<LevelAttemptData> _levelToAttemptsData;
        [JsonProperty] private string _playerName;

        public GameData(List<LevelAttemptData> levelToAttemptsData, string playerName)
        {
            _levelToAttemptsData = levelToAttemptsData;
            _playerName = playerName;
        }

        public bool TryGetData(out List<LevelAttemptData> levelToAttemptsData, out string playerName)
        {
            levelToAttemptsData = _levelToAttemptsData;
            playerName = _playerName;
            
            return levelToAttemptsData != null;
        }
    }
}