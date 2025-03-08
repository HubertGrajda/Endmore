using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [field: SerializeField] public Timer GameplayTimer { get; private set; }
        
        [SerializeField] private LevelGenerator levelGenerator;

        [SerializeField, Range(0f, 1f)] private float pointsPercentageToPassTheLevel;
        
        public event Action<int> OnLevelStarted;
        public event Action<int> OnCollisionsNumberChanged;
        
        private ScoreManager _scoreManager;
        private InputManager _inputManager;
        private GameManager _gameManager;

        public int CollisionsNumber { get; private set; }
        public int CurrentLevel { get; private set; }

        private const int INITIAL_LEVEL_NUMBER = 1;
        
        private void Start()
        {
            _scoreManager = ScoreManager.Instance;
            _gameManager = GameManager.Instance;
            _inputManager = InputManager.Instance;
            
            AddListeners();
            StartLevel(INITIAL_LEVEL_NUMBER);
            _inputManager.EnablePlayerActions();
        }

        private void OnDestroy()
        {
            _inputManager.DisablePlayerActions();
            RemoveListeners();
        }
        
        private void AddListeners()
        {
            _scoreManager.OnScoreTargetAchieved += OnScoreTargetAchieved;
        }

        private void RemoveListeners()
        {
            _scoreManager.OnScoreTargetAchieved -= OnScoreTargetAchieved;
        }
        
        private void OnScoreTargetAchieved(int obj)
        {
            EndCurrentLevel();
            StartLevel(CurrentLevel + 1);
        }
        
        private void EndCurrentLevel()
        {
            SaveAttempt();
            GameplayTimer.StopTimer();
            levelGenerator.ClearLevel();
        }

        private void SaveAttempt()
        {
            var attemptNumber = _gameManager.GetAttemptNumber(CurrentLevel) + 1;
            var attemptData = new LevelAttemptData(
                CurrentLevel,
                attemptNumber,
                CollisionsNumber,
                GameplayTimer.ElapsedTime);
            
            _gameManager.AddAttempt(CurrentLevel, attemptData);
        }

        private void StartLevel(int levelNumber)
        {
            if (!levelGenerator) return;
            
            ResetGameplay();
            CurrentLevel = levelNumber;
            levelGenerator.GenerateLevel(CurrentLevel);
            _scoreManager.SetScoreTarget(pointsPercentageToPassTheLevel);
            GameplayTimer.StartTimer();
            
            OnLevelStarted?.Invoke(CurrentLevel);
        }

        private void ResetGameplay()
        {
            SetCollisionsNumber(0);
            _scoreManager.ResetScore();
        }
        
        public void IncrementCollisionsNumber() => SetCollisionsNumber(CollisionsNumber + 1);

        private void SetCollisionsNumber(int value)
        {
            CollisionsNumber = value;
            OnCollisionsNumberChanged?.Invoke(CollisionsNumber);
        }
    }

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