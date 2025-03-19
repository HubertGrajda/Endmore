using Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class LevelAttemptVisualizer : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text levelNumberText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text collisionsText;
        
        private const string UNKNOWN_TEXT = "Unknown";
        private const string TIME_DISPLAY_FORMAT = @"mm\:ss";
        
        public void VisualizeAttempt(LevelAttemptData levelAttemptData)
        {
            levelAttemptData.GetData(out var levelNumber, out var playerName, out var collisions, out var time);
            
            VisualizeText(playerNameText, playerName);
            VisualizeText(levelNumberText, levelNumber.ToString());
            VisualizeText(collisionsText, collisions.ToString());
            VisualizeText(timeText, time.ToString(TIME_DISPLAY_FORMAT));
        }

        private void VisualizeText(TMP_Text text, string value)
        {
            if (text == null) return;
            value = string.IsNullOrEmpty(value) ? UNKNOWN_TEXT : value;
            
            text.text = value;
        }
    }
}