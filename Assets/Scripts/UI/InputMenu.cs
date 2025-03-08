using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Scripts.UI
{
    [RequireComponent(typeof(Canvas))]
    public class InputMenu : MonoBehaviour
    {
        [SerializeField] private InputActionReference input;
        [SerializeField] private Selectable selectableEntry;
        
        private Canvas _canvas;
        private InputManager _inputManager;
        private GameManager _gameManager;
        private EventSystem _eventSystem;
        
        private bool _isShown;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        
        private void Start()
        {
            _inputManager = InputManager.Instance;
            _gameManager = GameManager.Instance;
            _eventSystem = EventSystem.current;

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

            if (_isShown)
            {
                ToggleMenu(false);
            }
        }

        private void ProcessInput(InputAction.CallbackContext obj)
        {
            if (!_inputManager.GameplayUIInputs.enabled) return;
            
            ToggleMenu(!_isShown);
        }

        private void ToggleMenu(bool show)
        {
            _isShown = show;
            _canvas.enabled = show;
            
            if (show)
            {
                _eventSystem.SetSelectedGameObject(selectableEntry.gameObject);
                _gameManager.PauseGame();
                _inputManager.PlayerInputs.Disable();
            }
            else
            {
                _eventSystem.SetSelectedGameObject(null);
                _gameManager.ResumeGame();
                _inputManager.PlayerInputs.Enable();
            }
        }
    }
}