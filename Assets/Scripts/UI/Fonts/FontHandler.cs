using System;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    [RequireComponent(typeof(TMP_Text))]
    [DisallowMultipleComponent]
    public class FontHandler : MonoBehaviour
    {
        [Inject] private IFontService _fontService;
        
        private TMP_Text _text;
    
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            _fontService.AttachFontHandler(this);
        }

        public void ChangeFont(TMP_FontAsset font)
        {
            _text.font = font;
        }

        private void OnDestroy() => _fontService?.DetachFontHandler(this);
    }
}