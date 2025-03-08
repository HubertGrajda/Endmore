using System;
using Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text collisionsText;
        [SerializeField] private TMP_Text levelText;

        private ScoreManager _scoreManager;
        private GameplayManager _gameplayManager;
    
        private const string TIMER_DISPLAY_FORMAT = @"mm\:ss";
    
        private void Start()
        {
            _scoreManager = ScoreManager.Instance;
            _gameplayManager = GameplayManager.Instance;
            
            Refresh();
            AddListeners();
        }

        private void OnDestroy() => RemoveListeners();

        private void Refresh()
        {
            SetScoreText(_scoreManager.CurrentScore, _scoreManager.CurrentScoreTarget);
            SetCollisionsNumber(_gameplayManager.CollisionsNumber);
            SetTimerText(_gameplayManager.GameplayTimer.ElapsedTime);
            SetLevelText(_gameplayManager.CurrentLevel);
        }

        private void AddListeners()
        {
            _scoreManager.OnScoreChanged += OnScoreChanged;
            _scoreManager.OnScoreTargetChanged += OnScoreTargetChanged;
        
            _gameplayManager.OnCollisionsNumberChanged += OnCollisionsNumberChanged;
            _gameplayManager.OnLevelStarted += OnLevelStarted;
            _gameplayManager.GameplayTimer.OnSecondTick += OnGameplayTimeChanged;
        }

        private void RemoveListeners()
        {
            _scoreManager.OnScoreChanged -= OnScoreChanged;
            _scoreManager.OnScoreTargetChanged -= OnScoreTargetChanged;
        
            _gameplayManager.OnCollisionsNumberChanged -= OnCollisionsNumberChanged;
            _gameplayManager.OnLevelStarted -= OnLevelStarted;
            _gameplayManager.GameplayTimer.OnSecondTick -= OnGameplayTimeChanged;
        }
    
        private void OnGameplayTimeChanged(TimeSpan elapsedTime) => SetTimerText(elapsedTime);
    
        private void OnLevelStarted(int levelNumber) => SetLevelText(levelNumber);

        private void OnScoreChanged(int score) => SetScoreText(score, _scoreManager.CurrentScoreTarget);

        private void OnScoreTargetChanged(int scoreTarget) => SetScoreText(_scoreManager.CurrentScore, scoreTarget);
    
        private void OnCollisionsNumberChanged(int collisionsNumber) => SetCollisionsNumber(collisionsNumber);

        private void SetTimerText(TimeSpan elapsedTime)
        {
            if (!timerText) return;
        
            timerText.text = elapsedTime.ToString(TIMER_DISPLAY_FORMAT);
        }
        
        private void SetLevelText(int levelNumber)
        {
            if (!levelText) return;
        
            levelText.text = levelNumber.ToString();
        }
        
        private void SetScoreText(int score, int scoreTarget)
        {
            if (!scoreText) return;

            scoreText.text = $"{score}/{scoreTarget}";
        }
        
        private void SetCollisionsNumber(int collisionsNumber)
        {
            if (!collisionsText) return;
        
            collisionsText.text = collisionsNumber.ToString();
        }
    }
}