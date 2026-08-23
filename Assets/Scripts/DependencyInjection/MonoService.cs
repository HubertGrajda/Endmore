using Reflex.Core;
using UnityEngine;

namespace Scripts
{
    public abstract class MonoService : MonoBehaviour
    {
        public abstract void InstallBindings(ContainerBuilder builder);
    }
    
    public abstract class MonoService<TService> : MonoService
    {
        public override void InstallBindings(ContainerBuilder builder)
        {
            if (this is not TService service)
            {
                Debug.LogError($"[{name}] of type {GetType().Name} is not {typeof(TService).Name}");
                return;
            }
            
            builder.RegisterValue(service, new[] { typeof(TService) });
        }
    }
}