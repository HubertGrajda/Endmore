using UnityEngine;

namespace Scripts.UI
{
    public class HealthPointVisualizer : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int IsActive = Animator.StringToHash("IsActive");

        private bool _isActive = true;
        
        public void Activate()
        {
            if (_isActive) return;
            
            _isActive = true;
            animator.SetBool(IsActive, true);
        }

        public void Deactivate()
        {
            if (!_isActive) return;
            
            _isActive = false;
            animator.SetBool(IsActive, false);
        }
    }
}