using System;
using Reflex.Attributes;
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

        [Inject] private IGameService _gameService;
        [Inject] private IScoreService _scoreService;
        [Inject] private IGameplayService _gameplayManager;
    
        private const string TIMER_DISPLAY_FORMAT = @"mm\:ss";
    
        private void Start()
        {
            Refresh();
            AddListeners();
        }

        private void OnDestroy() => RemoveListeners();

        private void Refresh()
        {
            SetText(scoreText, $"{_scoreService.CurrentScore}/{_scoreService.CurrentScoreTarget}");
            SetText(timerText, _gameplayManager.GameplayTimer.ElapsedTime.ToString(TIMER_DISPLAY_FORMAT));
            SetText(collisionsText, _gameplayManager.CollisionsNumber.ToString());
            SetText(levelText, _gameplayManager.CurrentLevel.ToString());
            SetText(playerNameText, _gameService.PlayerName);
        }

        private void AddListeners()
        {
            _scoreService.OnScoreChanged += OnScoreChanged;
            _scoreService.OnScoreTargetChanged += OnScoreTargetChanged;
        
            _gameplayManager.OnCollisionsNumberChanged += OnCollisionsNumberChanged;
            _gameplayManager.OnLevelStarted += OnLevelStarted;
            _gameplayManager.GameplayTimer.OnSecondTick += OnGameplayTimeChanged;
        }

        private void RemoveListeners()
        {
            _scoreService.OnScoreChanged -= OnScoreChanged;
            _scoreService.OnScoreTargetChanged -= OnScoreTargetChanged;
        
            _gameplayManager.OnCollisionsNumberChanged -= OnCollisionsNumberChanged;
            _gameplayManager.OnLevelStarted -= OnLevelStarted;
            _gameplayManager.GameplayTimer.OnSecondTick -= OnGameplayTimeChanged;
        }
    
        private void OnGameplayTimeChanged(TimeSpan elapsedTime) =>
            SetText(timerText, elapsedTime.ToString(TIMER_DISPLAY_FORMAT));
    
        private void OnLevelStarted(int levelNumber) =>
            SetText(levelText, levelNumber.ToString());

        private void OnScoreChanged(int score) =>
            SetText(scoreText, $"{score}/{_scoreService.CurrentScoreTarget}");

        private void OnScoreTargetChanged(int scoreTarget) =>
            SetText(scoreText, $"{_scoreService.CurrentScore}/{scoreTarget}");
    
        private void OnCollisionsNumberChanged(int collisionsNumber) =>
            SetText(collisionsText, collisionsNumber.ToString());

        private void SetText(TMP_Text text, string value)
        {
            if (!text) return;
            
            text.text = value;
        }
    }
}