using Scripts.Gameplay;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SpawnableConfig), true)]
    public class ProjectileConfigInspector : UnityEditor.Editor
    {
        private const string SPRITE_PREVIEW_LABEL = "Sprite Preview:";
        private SpawnableConfig _spawnableConfig;
        
        public override void OnInspectorGUI()
        {
            _spawnableConfig = (SpawnableConfig)target;
            
            DrawDefaultInspector();
            DrawSpritePreview();
        }

        private void DrawSpritePreview()
        {
            _spawnableConfig.Sprite = (Sprite) EditorGUILayout.ObjectField(
                SPRITE_PREVIEW_LABEL, _spawnableConfig.Sprite, typeof(Sprite), false);
        }
    }
}