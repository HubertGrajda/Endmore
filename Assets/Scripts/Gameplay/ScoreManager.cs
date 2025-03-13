using System;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        [SerializeField, Range(0f, 1f)] private float pointsPercentageToPassTheLevel;
        
        public event Action<int> OnScoreChanged;
        public event Action<int> OnScoreTargetAchieved;
        public event Action<int> OnScoreTargetChanged;

        public int CurrentScore { get; private set; }
        public int CurrentScoreTarget { get; private set; }
        
        private int TotalPointsOnLevel { get; set; }

        private GameplayManager _gameplayManager;
        private GameplayManager GameplayManager => _gameplayManager == null 
            ? _gameplayManager = GameplayManager.Instance
            : _gameplayManager;
        
        private void Start()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            GameplayManager.OnLevelStarted += OnLevelStarted;
        }

        private void RemoveListeners()
        {
            GameplayManager.OnLevelStarted -= OnLevelStarted;
        }

        private void OnLevelStarted(int _)
        {
            SetScoreTarget(pointsPercentageToPassTheLevel);
        }
        
        public void AddScore(int score) => SetCurrentScore(CurrentScore + score);

        public void IncreaseTotalPointsBy(int points) => TotalPointsOnLevel += points;

        public void ResetScore()
        {
            TotalPointsOnLevel = 0;
            ResetCurrentScore();
            SetScoreTarget(0);
        }

        private void ResetCurrentScore()
        {
            CurrentScore = 0;
            OnScoreChanged?.Invoke(CurrentScore);
        }
        
        private void SetCurrentScore(int score)
        {
            CurrentScore = score;
            OnScoreChanged?.Invoke(CurrentScore);
            
            if (CurrentScoreTarget == 0) return;
            
            if (CurrentScore >= CurrentScoreTarget)
            {
                OnScoreTargetAchieved?.Invoke(CurrentScore);
            }
        }

        private void SetScoreTarget(float pointsPercentage)
        {
            CurrentScoreTarget = Mathf.RoundToInt(TotalPointsOnLevel * pointsPercentage);
            OnScoreTargetChanged?.Invoke(CurrentScoreTarget);
        }
    }
}