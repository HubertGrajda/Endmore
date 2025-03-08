using System;
using UnityEngine;

namespace Scripts
{
    public class ScoreManager : Singleton<ScoreManager>
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

        public void SetScoreTarget(float percentageOfTotalPoints)
        {
            CurrentScoreTarget = Mathf.RoundToInt(TotalPointsOnLevel * percentageOfTotalPoints);
            OnScoreTargetChanged?.Invoke(CurrentScoreTarget);
        }
    }
}