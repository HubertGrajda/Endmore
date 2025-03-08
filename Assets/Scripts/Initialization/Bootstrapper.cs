using UnityEngine;

namespace Scripts
{
    public static class Bootstrapper
    {
        private const string MANAGERS_PATH = "PersistentManagers";
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadPersistentManagers()
        {
            var managersPrefab = Resources.Load<GameObject>(MANAGERS_PATH);
            var managersInstance = Object.Instantiate(managersPrefab);
            Object.DontDestroyOnLoad(managersInstance);
        }
    }
}