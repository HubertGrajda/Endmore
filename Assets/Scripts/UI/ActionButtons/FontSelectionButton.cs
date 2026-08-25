using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class FontSelectionButton : ActionButton
    {
        [SerializeField] private TMP_FontAsset fontAsset;
        
        [Inject] private IFontService _fontService;
        
        private TMP_Text[] _buttonTexts;

        protected override bool IsValid => fontAsset != null && _fontService != null; 

        protected override void Prepare()
        {
            PrepareButtonTexts();
        }
        
        protected override void OnClick()
        {
            if (_fontService.CurrentFont == fontAsset) return;
            
            _fontService.SetFont(fontAsset);
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