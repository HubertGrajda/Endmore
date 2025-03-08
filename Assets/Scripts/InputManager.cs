namespace Scripts
{
    public class InputManager : Singleton<InputManager>
    {
        public InputActions.PlayerActions PlayerInputs { get; private set; }
        public InputActions.GameplayUIActions GameplayUIInputs { get; private set; }
        
        private InputActions _inputs;
        private ScenesManager _scenesManager;

        protected override void Awake()
        {
            base.Awake();
            
            _inputs = new InputActions();
            GameplayUIInputs = _inputs.GameplayUI;
            PlayerInputs = _inputs.Player;
        }

        private void Start()
        {
            _scenesManager = ScenesManager.Instance;
            
            AddListeners();
        }
        
        private void OnDestroy()
        {
            RemoveListeners();
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
            EnableGameplayUIActions();
        }
        
        private void OnSceneChanged()
        {
            DisableGameplayUIActions();
        }

        public void EnablePlayerActions() => PlayerInputs.Enable();
        public void DisablePlayerActions() => PlayerInputs.Disable();
        
        private void EnableGameplayUIActions() => GameplayUIInputs.Enable();
        private void DisableGameplayUIActions() => GameplayUIInputs.Disable();
    }
}