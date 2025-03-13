using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.UI
{
    [RequireComponent(typeof(Canvas))]
    public class InputMenu : Menu
    {
        [SerializeField] private InputActionReference input;
        
        protected override void Start()
        {
            base.Start();
            
            if (input)
            {
                input.action.started += ProcessInput;
            }
        }

        private void OnDestroy()
        {
            if (input)
            {
                input.action.started -= ProcessInput;
            }

            if (IsShown)
            {
                ToggleMenu(false);
            }
        }

        private void ProcessInput(InputAction.CallbackContext obj)
        {
            if (!InputManager.GameplayUIInputs.enabled) return;
            
            ToggleMenu(!IsShown);
        }
    }
}