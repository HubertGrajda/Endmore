using Reflex.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Selectable selectableEntry;
        
        protected bool IsShown { get; private set; }
        [Inject] protected IInputService InputService { get; private set; }

        [Inject] private IPauseService _pauseService;
        
        private EventSystem _eventSystem;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        
        protected virtual void Start()
        {
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
            _pauseService.AddPauseSource(this);
            InputService.GameplayActions.Disable();
        }
        
        private void OnHide()
        {
            _eventSystem.SetSelectedGameObject(null);
            _pauseService.RemovePauseSource(this);
            InputService.GameplayActions.Enable();
        }
    }
}