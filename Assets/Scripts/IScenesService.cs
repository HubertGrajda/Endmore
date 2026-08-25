using System;

namespace Scripts
{
    public interface IScenesService
    { 
        void LaunchSceneByName(string sceneName);
        void ReloadActiveScene();
        event Action OnSceneChange;
        event Action OnSceneChanged;
    }
}