using System;
using Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text collisionsText;
        [SerializeField] private TMP_Text levelText;

        private ScoreManager _scoreManager;
        private GameplayManager _gameplayManager;
        private GameManager _gameManager;
    
        private const string TIMER_DISPLAY_FORMAT = @"mm\:ss";
    
        private void Start()
        {
            _scoreManager = ScoreManager.Instance;
            _gameplayManager = GameplayManager.Instance;
            _gameManager = GameManager.Instance;
            
            Refresh();
            AddListeners();
        }

        private void OnDestroy() => RemoveListeners();

        private void Refresh()
        {
            SetText(scoreText, $"{_scoreManager.CurrentScore}/{_scoreManager.CurrentScoreTarget}");
            SetText(timerText, _gameplayManager.GameplayTimer.ElapsedTime.ToString(TIMER_DISPLAY_FORMAT));
            SetText(collisionsText, _gameplayManager.CollisionsNumber.ToString());
            SetText(levelText, _gameplayManager.CurrentLevel.ToString());
            SetText(playerNameText, _gameManager.PlayerName);
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
    
        private void OnGameplayTimeChanged(TimeSpan elapsedTime) =>
            SetText(timerText, elapsedTime.ToString(TIMER_DISPLAY_FORMAT));
    
        private void OnLevelStarted(int levelNumber) =>
            SetText(levelText, levelNumber.ToString());

        private void OnScoreChanged(int score) =>
            SetText(scoreText, $"{score}/{_scoreManager.CurrentScoreTarget}");

        private void OnScoreTargetChanged(int scoreTarget) =>
            SetText(scoreText, $"{_scoreManager.CurrentScore}/{scoreTarget}");
    
        private void OnCollisionsNumberChanged(int collisionsNumber) =>
            SetText(collisionsText, collisionsNumber.ToString());

        private void SetText(TMP_Text text, string value)
        {
            if (!text) return;
            
            text.text = value;
        }
    }
}