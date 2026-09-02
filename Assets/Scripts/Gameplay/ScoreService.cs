using System;
using UnityEngine;

namespace Scripts
{
    public class ScoreService : IScoreService
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnScoreTargetAchieved;
        public event Action<int> OnScoreTargetChanged;

        public int CurrentScore { get; private set; }
        public int CurrentScoreTarget { get; private set; }

        private int TotalPointsOnLevel { get; set; }

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

        public void SetScoreTarget(float pointsPercentage)
        {
            CurrentScoreTarget = Mathf.RoundToInt(TotalPointsOnLevel * pointsPercentage);
            OnScoreTargetChanged?.Invoke(CurrentScoreTarget);
        }
    }

    public interface IScoreService
    {
        event Action<int> OnScoreChanged;
        event Action<int> OnScoreTargetChanged;
        event Action<int> OnScoreTargetAchieved;
        
        int CurrentScore { get; }
        int CurrentScoreTarget { get; }

        void AddScore(int configCoinValue);
        void IncreaseTotalPointsBy(int configCoinValue);
        void SetScoreTarget(float pointsPercentageToPassTheLevel);
        void ResetScore();
    }
}