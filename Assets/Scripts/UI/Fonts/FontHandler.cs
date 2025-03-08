using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    [RequireComponent(typeof(TMP_Text))]
    [DisallowMultipleComponent]
    public class FontHandler : MonoBehaviour
    {
        private TMP_Text _text;
        private FontManager _fontManager;
    
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _fontManager = FontManager.Instance;
            _fontManager.AttachFontHandler(this);
        }

        public void ChangeFont(TMP_FontAsset font)
        {
            _text.font = font;
        }

        private void OnDestroy() => _fontManager.DetachFontHandler(this);
    }
}