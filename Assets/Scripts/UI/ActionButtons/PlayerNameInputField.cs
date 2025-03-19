using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        private GameManager _gameManager;
        private TMP_InputField _inputField; 
        
        protected void Start()
        {
            _gameManager = GameManager.Instance;
            _inputField = GetComponent<TMP_InputField>();
            _inputField.text = _gameManager.PlayerName;
            _inputField.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnEndEdit(string value)
        {
            _gameManager.ChangePlayerName(value);
        }
    }
}