using Reflex.Attributes;
using Scripts.CameraManagement;
using UnityEngine;

namespace Scripts.UI
{
    public class MoveCameraButton : ActionButton
    {
        [SerializeField] private Transform target;
        [SerializeField] private float duration;
        
        [Inject] private ICameraService _cameraService;
        
        protected override bool IsValid => _cameraService != null && target != null;
        
        protected override void OnClick()
        {
            _cameraService.MoveCamera(target.position, duration);
        }
    }
}