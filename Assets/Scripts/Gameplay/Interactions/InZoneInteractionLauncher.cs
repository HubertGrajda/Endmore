using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Scripts.Gameplay
{
    public class InZoneInteractionLauncher : InteractionLauncher
    {
        [Header("Text options")]
        [SerializeField] private TMP_Text hintText;
        [SerializeField] private float toggleAnimationDuration = 0.5f;
        
        [Header("Events")]
        [SerializeField] private UnityEvent onZoneEntered;
        [SerializeField] private UnityEvent onZoneLeft;
        
        private InputAction _interactionInput;
        private GameObject _triggerObject;
        private const string PLAYER_TAG = "Player";
        
        private void Start()
        {
            _interactionInput = InputManager.Instance.PlayerInputs.Interact;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Interactables.Count == 0) return;
            if (!other.CompareTag(PLAYER_TAG)) return;
            
            _triggerObject = other.gameObject;
            ActivateZone();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (Interactables.Count == 0) return;
            if (other.gameObject != _triggerObject) return;
            
            _triggerObject = null;
            
           DeactivateZone();
        }

        private void ActivateZone()
        {
            EnableInteraction();
            ToggleTextAnimation(true);
            onZoneEntered?.Invoke();
        }
        
        private void DeactivateZone()
        {
            DisableInteraction();
            ToggleTextAnimation(false);
            onZoneLeft?.Invoke();
        }

        private void ToggleTextAnimation(bool show)
        {
            if (!hintText) return;
            
            var endAlphaValue = show ? 1f : 0f;
            hintText.DOFade(endAlphaValue, toggleAnimationDuration).SetAutoKill(true);
        }
        
        private void EnableInteraction()
        {
            if (_interactionInput == null) return;
            
            _interactionInput.Enable();
            _interactionInput.started += OnInputStarted;
        }

        private void DisableInteraction()
        {
            if (_interactionInput == null) return;
            
            _interactionInput.started -= OnInputStarted;
            _interactionInput.Disable();
        }

        private void OnDisable()
        {
            DisableInteraction();
        }

        private void OnInputStarted(InputAction.CallbackContext obj)
        {
            if (Interactables.Count == 0) return;

            foreach (var interactable in Interactables)
            {
                interactable.Interact(_triggerObject);
            }

            if (Interactables.Count == 0) return;
            
            DeactivateZone();
        }
    }
}