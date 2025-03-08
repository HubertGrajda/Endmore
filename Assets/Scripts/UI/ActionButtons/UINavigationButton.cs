using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class UINavigationButton : ActionButton
    {
        [SerializeField] private GameObject objectToSelect;
        
        private EventSystem _eventSystem;

        protected override bool IsValid => objectToSelect != null && _eventSystem != null;
        
        protected override void Prepare()
        {
            _eventSystem = EventSystem.current;
        }
        
        protected override void OnClick()
        {
            _eventSystem.SetSelectedGameObject(objectToSelect);
        }
    }
}