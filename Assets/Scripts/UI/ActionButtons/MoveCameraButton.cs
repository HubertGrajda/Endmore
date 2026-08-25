using Reflex.Attributes;
using Scripts.CameraManagement;
using UnityEngine;

namespace Scripts.UI
{
    public class MoveCameraButton : ActionButton
    {
        [SerializeField] private Transform target;
        [SerializeField] private float duration;
        
        [Inject] private ICameraService _cameraManager;
        
        protected override bool IsValid => _cameraManager != null && target != null;
        
        protected override void OnClick()
        {
            _cameraManager.MoveCamera(target.position, duration);
        }
    }
}