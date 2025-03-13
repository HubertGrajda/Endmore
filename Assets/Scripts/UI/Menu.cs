using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Selectable selectableEntry;
        
        protected bool IsShown { get; private set; }
        protected InputManager InputManager { get; private set; }

        private GameManager _gameManager;
        private EventSystem _eventSystem;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        
        protected virtual void Start()
        {
            InputManager = InputManager.Instance;
            _gameManager = GameManager.Instance;
            _eventSystem = EventSystem.current;
        }
        
        protected void ToggleMenu(bool show)
        {
            IsShown = show;
            _canvas.enabled = show;
            
            if (show)
            {
                OnShow();
            }
            else
            {
                OnHide();
            }
        }
        
        private void OnShow()
        {
            _eventSystem.SetSelectedGameObject(selectableEntry.gameObject);
            _gameManager.PauseGame();
            InputManager.PlayerInputs.Disable();
        }
        
        private void OnHide()
        {
            _eventSystem.SetSelectedGameObject(null);
            _gameManager.ResumeGame();
            InputManager.PlayerInputs.Enable();
        }
    }
}