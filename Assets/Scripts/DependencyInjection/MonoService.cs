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

            if (typeof(TService) == service.GetType())
            {
                builder.RegisterValue(service);
                return;
            }
            
            builder.RegisterValue(service, new[] { typeof(TService) });
        }
    }
}