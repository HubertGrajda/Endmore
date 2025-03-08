using Scripts.CameraManagement;
using UnityEngine;

namespace Scripts.UI
{
    public class MoveCameraButton : ActionButton
    {
        [SerializeField] private Transform target;
        [SerializeField] private float duration;
        
        private CameraManager _cameraManager;
        
        protected override bool IsValid => _cameraManager != null && target != null;
        
        protected override void Prepare()
        {
            _cameraManager = CameraManager.Instance;
        }

        protected override void OnClick()
        {
            _cameraManager.MoveCamera(target.position, duration);
        }
    }
}