using Reflex.Attributes;

namespace Scripts
{
    public class InputService : MonoService<IInputService>, IInputService
    {
        public InputActions.PlayerActions GameplayActions { get; private set; }
        public InputActions.GameplayUIActions UIActions { get; private set; }
        
        private InputActions _inputs;
        
        [Inject] private IScenesService _scenesManager;

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
            _scenesManager.OnSceneChange += OnSceneChange;
            _scenesManager.OnSceneChanged += OnSceneChanged;
        }

        private void RemoveListeners()
        {
            _scenesManager.OnSceneChange -= OnSceneChange;
            _scenesManager.OnSceneChanged -= OnSceneChanged;
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