using System.Collections;
using UnityEngine;

namespace Scripts.CameraManagement
{
    public class CameraManager : Singleton<CameraManager>
    {
        public Camera Camera { get; private set; }

        private ScenesManager _scenesManager;
        private Coroutine _movementCoroutine;

        private int _currentWidth = NATIVE_WIDTH;
        private int _currentHeight = NATIVE_HEIGHT;
        private float _orthographicSize;

        private const int NATIVE_WIDTH = 1920;
        private const int NATIVE_HEIGHT = 1080;
        
        private const float NATIVE_ASPECT_RATIO = (float)NATIVE_WIDTH / NATIVE_HEIGHT;
        private const float MIN_ASPECT_RATIO = 4f / 3f;
        
        private void Start()
        {
            _scenesManager = ScenesManager.Instance;
            Camera = Camera.main;
            
            if (!Camera) return;
            
            _orthographicSize = Camera.orthographicSize;
            AddListeners();
        }

        private void Update()
        {
            if (_currentWidth == Screen.width && _currentHeight == Screen.height) return;

            AdjustCameraSize();
        }

        private void AdjustCameraSize()
        {
            var currentAspect = (float)Screen.width / Screen.height;
            var cameraSizeMultiplier =
                NATIVE_ASPECT_RATIO / Mathf.Clamp(currentAspect, MIN_ASPECT_RATIO, NATIVE_ASPECT_RATIO);
            
            Camera.orthographicSize = _orthographicSize * cameraSizeMultiplier;
            _currentWidth = Screen.width;
            _currentHeight = Screen.height;
        }

        private void AddListeners()
        {
            _scenesManager.OnSceneChange += OnSceneChange;
            _scenesManager.OnSceneChanged += OnSceneChanged;
        }

        private void RemoveListeners()
        {
            _scenesManager.OnSceneChange -= OnSceneChange;
            _scenesManager.OnSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChange() => StopAllCoroutines();
        
        private void OnSceneChanged()
        {
            Camera = Camera.main;

            if (!Camera) return;
            
            _orthographicSize = Camera.orthographicSize;
            AdjustCameraSize();
        }

        public void MoveCamera(Vector3 targetPosition, float duration)
        {
            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
            }
            
            _movementCoroutine = StartCoroutine(CameraMovementCoroutine(targetPosition, duration));
        }
    
        private IEnumerator CameraMovementCoroutine(Vector3 targetPosition, float duration)
        {
            if (!Camera) yield break;
            
            var timer = 0f;
            var startPosition = Camera.transform.position;
            targetPosition.z = startPosition.z;
            
            while (timer <= duration)
            {
                var position = Vector3.Lerp(startPosition, targetPosition, timer / duration);
                Camera.transform.position = position;
                
                timer += Time.deltaTime;
                yield return null;
            }

            Camera.transform.position = targetPosition;
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}