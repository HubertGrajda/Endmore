using System;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [field: SerializeField] public Timer GameplayTimer { get; private set; }
        
        [SerializeField] private LevelGenerator levelGenerator;
        
        [SerializeField] private float restartDelayAfterDeath = 1f;
        
        public event Action<int> OnLevelStarted;
        public event Action<int> OnCollisionsNumberChanged;
        
        public int CollisionsNumber { get; private set; }
        public int CurrentLevel { get; private set; }
        
        private ScoreManager _scoreManager;
        private GameManager _gameManager;
        
        private PlayerController _playerController;
        private PlayerHealthSystem _playerHealthSystem;

        private bool _gameplayEnded;
        
        private const int INITIAL_LEVEL_NUMBER = 1;
        
        private void Start()
        {
            _scoreManager = ScoreManager.Instance;
            _gameManager = GameManager.Instance;
            _playerController = PlayerController.Instance;
            _playerHealthSystem = _playerController.PlayerHealthSystem;
            
            AddListeners();
            StartGameplay();
        }

        private void StartGameplay()
        {
            StartLevel(INITIAL_LEVEL_NUMBER);
            GameplayTimer.StartTimer();
            _gameplayEnded = false;
        }
        
        private void StartLevel(int levelNumber)
        {
            if (!levelGenerator) return;
            
            CurrentLevel = levelNumber;
            levelGenerator.GenerateLevel(CurrentLevel);
            
            OnLevelStarted?.Invoke(CurrentLevel);
        }
        
        private void OnDestroy()
        {
            if (!_gameplayEnded)
            {
                EndGameplay();
            }
            
            RemoveListeners();
        }
        
        private void AddListeners()
        {
            _scoreManager.OnScoreTargetAchieved += OnScoreTargetAchieved;
            _playerHealthSystem.OnDeath += OnPlayerDeath;
        }

        private void RemoveListeners()
        {
            _scoreManager.OnScoreTargetAchieved -= OnScoreTargetAchieved;
            _playerHealthSystem.OnDeath -= OnPlayerDeath;
        }
        
        private void OnPlayerDeath()
        {
            EndGameplay();
            Invoke(nameof(RestartGameplay), restartDelayAfterDeath);
        }
        
        private void EndGameplay()
        {
            SaveAttempt();
            ClearGameplay();
            GameplayTimer.StopTimer();
            _gameplayEnded = true;
        }
        
        private void ClearGameplay()
        {
            ClearLevel();
            SetCollisionsNumber(0);
        }
        
        private void RestartGameplay()
        {
            ScenesManager.Instance.ReloadActiveScene();
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
        
        private void OnScoreTargetAchieved(int obj)
        {
            ClearLevel();
            StartLevel(CurrentLevel + 1);
        }

        private void ClearLevel()
        {
            _scoreManager.ResetScore();
            levelGenerator.ClearLevel();
        }

        public void IncrementCollisionsNumber() => SetCollisionsNumber(CollisionsNumber + 1);

        private void SetCollisionsNumber(int value)
        {
            CollisionsNumber = value;
            OnCollisionsNumberChanged?.Invoke(CollisionsNumber);
        }
    }
}