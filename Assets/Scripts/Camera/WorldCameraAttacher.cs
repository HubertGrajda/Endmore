using Reflex.Attributes;
using UnityEngine;

namespace Scripts.CameraManagement
{
    [RequireComponent(typeof(Canvas))]
    public class WorldCameraAttacher : MonoBehaviour
    {
        [Inject] private ICameraService _cameraService;
        
        private Canvas _canvas;
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            _canvas.worldCamera = _cameraService.Camera;
        }
    }
}