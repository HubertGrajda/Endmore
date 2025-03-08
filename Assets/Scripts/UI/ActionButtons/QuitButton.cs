using UnityEngine;

namespace Scripts.UI
{
    public class QuitButton : ActionButton
    {
        protected override void OnClick()
        {
#if UNITY_EDITOR
            Debug.Log("Game closed.");
#endif
            
            Application.Quit();
        }
    }
}