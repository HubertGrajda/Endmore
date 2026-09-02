using System;
using Reflex.Attributes;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class GameplayService : MonoService<IGameplayService>, IGameplayService
    {
        public event Action OnLevelClear;
        public event Action<int> OnLevelStarted;
        public event Action<int> OnCollisionsNumberChanged;
        
        [field: SerializeField] public Timer GameplayTimer { get; private set; }
        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private float restartDelayAfterDeath = 1f;
        [SerializeField, Range(0f, 1f)] private float pointsPercentageToPassTheLevel = 1;
        
        [Inject] private IScoreService _scoreService;
        [Inject] private IGameService _gameService;
        [Inject] private IScenesService _scenesService;
        
        private bool _gameplayStarted;
        private bool _gameplayEnded;
        
        private const int INITIAL_LEVEL_NUMBER = 1;
        
        public int CollisionsNumber { get; private set; }
        public int CurrentLevel { get; private set; }
        
        public bool IsDuringGameplay => _gameplayStarted && !_gameplayEnded;
        
        private void Start()
        {
            AddListeners();
            StartGameplay();
        }

        private void StartGameplay()
        {
            StartLevel(INITIAL_LEVEL_NUMBER);
            GameplayTimer.StartTimer();
            _gameplayEnded = false;
            _gameplayStarted = true;
        }
        
        private void StartLevel(int levelNumber)
        {
            if (!levelGenerator) return;
            
            CurrentLevel = levelNumber;
            levelGenerator.GenerateLevel(CurrentLevel);
            _scoreService.SetScoreTarget(pointsPercentageToPassTheLevel); 
            
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
            _scoreService.OnScoreTargetAchieved += OnScoreTargetAchieved;
        }

        private void RemoveListeners()
        {
            _scoreService.OnScoreTargetAchieved -= OnScoreTargetAchieved;
        }
        
        public void FinishAndRestart()
        {
            EndGameplay();
            Invoke(nameof(RestartGameplay), restartDelayAfterDeath);
        }
        
        private void EndGameplay()
        {
            SaveAttempt();
            ClearGameplay();
            GameplayTimer.StopTimer();
            
            _gameplayStarted = false;
            _gameplayEnded = true;
        }
        
        private void ClearGameplay()
        {
            ClearLevel();
            SetCollisionsNumber(0);
        }
        
        private void RestartGameplay()
        {
            _scenesService.ReloadActiveScene();
        }

        private void SaveAttempt()
        {
            if (CurrentLevel <= 1) return;
            
            var attemptData = new LevelAttemptData(
                CurrentLevel,
                _gameService.PlayerName,
                CollisionsNumber,
                GameplayTimer.ElapsedTime);
            
            _gameService.AddAttempt(attemptData);
        }
        
        private void OnScoreTargetAchieved(int obj)
        {
            ClearLevel();
            StartLevel(CurrentLevel + 1);
        }

        private void ClearLevel()
        {
            _scoreService.ResetScore();
            levelGenerator.ClearLevel();
            OnLevelClear?.Invoke();
        }

        public void IncrementCollisionsNumber() => SetCollisionsNumber(CollisionsNumber + 1);

        private void SetCollisionsNumber(int value)
        {
            CollisionsNumber = value;
            OnCollisionsNumberChanged?.Invoke(CollisionsNumber);
        }
    }
}