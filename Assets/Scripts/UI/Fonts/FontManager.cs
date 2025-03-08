using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class FontManager : Singleton<FontManager>
    {
        [SerializeField] private TMP_FontAsset defaultFont;
        [SerializeField] private List<TMP_FontAsset> availableFonts;
        
        public TMP_FontAsset CurrentFont { get; private set; }

        private readonly List<FontHandler> _fontHandlers = new();
        
        private const string SELECTED_FONT_KEY = "SelectedFont";
        
        private void Start()
        {
            var initialFont = TryGetFontFromPlayerPrefs(out var selectedFont) ? selectedFont : defaultFont;
            
            SetFont(initialFont);
        }

        public void AttachFontHandler(FontHandler fontHandler)
        {
            if (_fontHandlers.Contains(fontHandler)) return;
            
            _fontHandlers.Add(fontHandler);
            fontHandler.ChangeFont(CurrentFont);
        }
        
        public void DetachFontHandler(FontHandler fontHandler)
        {
            if (_fontHandlers.Contains(fontHandler)) return;
            
            _fontHandlers.Remove(fontHandler);
        }

        public void SetFont(TMP_FontAsset fontAsset)
        {
            CurrentFont = fontAsset;
            PlayerPrefs.SetString(SELECTED_FONT_KEY, fontAsset.name);
            
            foreach (var fontHandler in _fontHandlers)
            {
                fontHandler.ChangeFont(fontAsset);
            }
        }

        private bool TryGetFontFromPlayerPrefs(out TMP_FontAsset fontAsset)
        {
            fontAsset = default;
            
            if (!PlayerPrefs.HasKey(SELECTED_FONT_KEY)) return false;
            
            var selectedFontName = PlayerPrefs.GetString(SELECTED_FONT_KEY);
            fontAsset = availableFonts.FirstOrDefault(x => x.name == selectedFontName);
            
            return fontAsset != null;
        }
    }
}