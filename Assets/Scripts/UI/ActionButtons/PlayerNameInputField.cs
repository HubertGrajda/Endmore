using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        [Inject] private IGameService _gameService;
        
        private TMP_InputField _inputField; 
        
        protected void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.text = _gameService.PlayerName;
            _inputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnEndEdit(string value)
        {
            _gameService.ChangePlayerName(value);
        }
    }
}