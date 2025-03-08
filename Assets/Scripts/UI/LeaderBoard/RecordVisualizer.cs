using Scripts.Gameplay;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class RecordVisualizer : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelNumberText;
        [SerializeField] private TMP_Text attemptNumberText;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text collisionsText;
        
        private const string TIME_DISPLAY_FORMAT = @"mm\:ss";
        
        public void VisualizeAttempt(LevelAttemptData levelAttemptData)
        {
            levelAttemptData.GetData(out var levelNumber, out var attemptNumber, out var collisions, out var time);
            
            VisualizeText(levelNumberText, levelNumber.ToString());
            VisualizeText(attemptNumberText, attemptNumber.ToString());
            VisualizeText(collisionsText, collisions.ToString());
            VisualizeText(timeText, time.ToString(TIME_DISPLAY_FORMAT));
        }

        private void VisualizeText(TMP_Text text, string value)
        {
            if (text == null) return;
            
            text.text = value;
        }
    }
}