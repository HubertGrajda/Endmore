using Reflex.Attributes;

namespace Scripts
{
    public class InputService : MonoService<IInputService>, IInputService
    {
        public InputActions.PlayerActions GameplayActions { get; private set; }
        public InputActions.GameplayUIActions UIActions { get; private set; }
        
        private InputActions _inputs;
        
        [Inject] private IScenesService _scenesService;

        protected void Awake()
        {
            _inputs = new InputActions();
            UIActions = _inputs.GameplayUI;
            GameplayActions = _inputs.Player;
        }

        private void Start()
        {
            AddListeners();
        }
        
        private void OnDestroy()
        {
            RemoveListeners();
            _inputs.Dispose();
        }

        private void OnEnable() => _inputs?.Enable();

        private void OnDisable() => _inputs?.Disable();
        
        private void AddListeners()
        {
            _scenesService.OnSceneChange += OnSceneChange;
            _scenesService.OnSceneChanged += OnSceneChanged;
        }

        private void RemoveListeners()
        {
            _scenesService.OnSceneChange -= OnSceneChange;
            _scenesService.OnSceneChanged -= OnSceneChanged;
        }
        
        private void OnSceneChange()
        {
            _inputs?.Disable();
        }
        
        private void OnSceneChanged()
        {
           _inputs?.Enable();
        }
    }

    public interface IInputService
    {
        InputActions.PlayerActions GameplayActions { get; }
        InputActions.GameplayUIActions UIActions { get; }
    }
}