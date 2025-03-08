using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ActionButton : MonoBehaviour
    {
        private Button Button { get; set; }
        
        protected virtual bool IsValid => true;
        
        private void Awake()
        {
            Button = GetComponent<Button>();
            Prepare();
            
            if (!IsValid) return;
            
            Button.onClick.AddListener(OnClick);
        }

        protected virtual void Prepare()
        {
        }

        protected abstract void OnClick();

    }
}