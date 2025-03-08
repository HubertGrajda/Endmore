using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class FontSelectionButton : ActionButton
    {
        [SerializeField] private TMP_FontAsset fontAsset;
        
        private FontManager _fontManager;
        private TMP_Text[] _buttonTexts;

        protected override bool IsValid => fontAsset != null && _fontManager != null; 

        protected override void Prepare()
        {
            _fontManager = FontManager.Instance;
            PrepareButtonTexts();
        }
        
        protected override void OnClick()
        {
            if (_fontManager.CurrentFont == fontAsset) return;
            
            _fontManager.SetFont(fontAsset);
        }

        private void PrepareButtonTexts()
        {
            _buttonTexts = GetComponentsInChildren<TMP_Text>();
            
            foreach (var text in _buttonTexts)
            {
                text.font = fontAsset;
            }
        }
    }
}