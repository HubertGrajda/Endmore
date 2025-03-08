using Scripts.Gameplay;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGeneratorInspector : UnityEditor.Editor
    {
        private const string REGENERATE_LEVEL_LABEL = "Regenerate Level";
        private LevelGenerator _spawnableConfig;
        
        public override void OnInspectorGUI()
        {
            _spawnableConfig = (LevelGenerator)target;
            
            DrawDefaultInspector();
            DrawRegenerateButton();
        }

        private void DrawRegenerateButton()
        {
            if (!Application.isPlaying) return;
            
            if (GUILayout.Button(REGENERATE_LEVEL_LABEL))
            {
                _spawnableConfig.Recreate();
            }
        }
    }
}