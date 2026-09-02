using Reflex.Core;
using UnityEngine;

namespace Scripts
{
    public class GameplaySceneInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            var gameplayMonoServices = GetComponentsInChildren<MonoService>();
            
            foreach (var monoService in gameplayMonoServices)
            {
                monoService.InstallBindings(builder);
            }
        }
    }
}