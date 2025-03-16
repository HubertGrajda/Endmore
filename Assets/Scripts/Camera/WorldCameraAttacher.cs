using UnityEngine;

namespace Scripts.CameraManagement
{
    [RequireComponent(typeof(Canvas))]
    public class WorldCameraAttacher : MonoBehaviour
    {
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            _canvas.worldCamera = CameraManager.Instance.Camera;
        }
    }
}