using System;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [field: SerializeField] public Timer GameplayTimer { get; private set; }
        
        [SerializeField] private LevelGenerator levelGenerator;

        [SerializeField, Range(0f, 1f)] private float pointsPercentageToPassTheLevel;
        [SerializeField] private float restartDelayAfterDeath = 1f;
        
        public event Action<int> OnLevelStarted;
        public event Action<int> OnCollisionsNumberChanged;
        
        public int CollisionsNumber { get; private set; }
        public int CurrentLevel { get; private set; }
        
        private ScoreManager _scoreManager;
        private InputManager _inputManager;
        private GameManager _gameManager;

        private const int INITIAL_LEVEL_NUMBER = 1;
        
        private PlayerController _playerController;
        private PlayerHitsHandler _playerHitsHandler;
        
        private void Start()
        {
            _scoreManager = ScoreManager.Instance;
            _gameManager = GameManager.Instance;
            _inputManager = InputManager.Instance;
            _playerController = PlayerController.Instance;
            
            _playerHitsHandler = _playerController.PlayerHitsHandler;
            
            _inputManager.EnablePlayerActions();
            
            AddListeners();
            StartGameplay();
        }

        private void StartGameplay()
        {
            SetCollisionsNumber(0);
            StartLevel(INITIAL_LEVEL_NUMBER);
            GameplayTimer.StartTimer();
        }
        
        private void StartLevel(int levelNumber)
        {
            if (!levelGenerator) return;
            
            CurrentLevel = levelNumber;
            _scoreManager.ResetScore();
            levelGenerator.GenerateLevel(CurrentLevel);
            _scoreManager.SetScoreTarget(pointsPercentageToPassTheLevel);
            
            OnLevelStarted?.Invoke(CurrentLevel);
        }
        
        private void OnDestroy()
        {
            _inputManager.DisablePlayerActions();
            RemoveListeners();
        }
        
        private void AddListeners()
        {
            _scoreManager.OnScoreTargetAchieved += OnScoreTargetAchieved;
            _playerHitsHandler.OnDeath += OnPlayerDeath;
        }

        private void RemoveListeners()
        {
            _scoreManager.OnScoreTargetAchieved -= OnScoreTargetAchieved;
            _playerHitsHandler.OnDeath -= OnPlayerDeath;
        }
        
        private void OnPlayerDeath()
        {
            EndGameplay();
            Invoke(nameof(RestartGameplay), restartDelayAfterDeath);
        }
        
        private void EndGameplay()
        {
            SaveAttempt();
            GameplayTimer.StopTimer();
        }

        private void RestartGameplay()
        {
            ScenesManager.Instance.RestartLevel();
        }
        
        private void OnScoreTargetAchieved(int obj)
        {
            levelGenerator.ClearLevel();
            StartLevel(CurrentLevel + 1);
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

        public void IncrementCollisionsNumber() => SetCollisionsNumber(CollisionsNumber + 1);

        private void SetCollisionsNumber(int value)
        {
            CollisionsNumber = value;
            OnCollisionsNumberChanged?.Invoke(CollisionsNumber);
        }
    }
}