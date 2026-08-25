using System.Collections.Generic;
using Reflex.Core;
using Reflex.Enums;
using Reflex.Injectors;
using Scripts.SaveSystem;
using UnityEngine;
using Resolution = Reflex.Enums.Resolution;

namespace Scripts
{
    public class GameInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private List<MonoService> servicesPrefabs;

        private const string SERVICES_KEEPER_NAME = "Services";
        private GameObject _servicesKeeper;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            _servicesKeeper = new GameObject(SERVICES_KEEPER_NAME);
            _servicesKeeper.SetActive(false);
            
            InstallMonoServices(builder);
            InstallServices(builder);
            
            builder.OnContainerBuilt += OnContainerBuilt;
        }

        private void OnContainerBuilt(Container container)
        {
            GameObjectInjector.InjectRecursive(_servicesKeeper, container);
            
            _servicesKeeper.SetActive(true);
            DontDestroyOnLoad(_servicesKeeper);
        }

        private void InstallMonoServices(ContainerBuilder builder)
        {
            foreach (var servicePrefab in servicesPrefabs)
            {
                if (servicePrefab == null) continue;

                var serviceInstance = Instantiate(servicePrefab, _servicesKeeper.transform);
                serviceInstance.InstallBindings(builder);
            }
        }

        private void InstallServices(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(SaveService),  new[] { typeof(ISaveService) }, Lifetime.Singleton, Resolution.Eager);
            builder.RegisterType(typeof(PauseService),  new[] { typeof(IPauseService) }, Lifetime.Singleton, Resolution.Lazy);
            builder.RegisterType(typeof(TimeService),  new[] { typeof(ITimeService) }, Lifetime.Singleton, Resolution.Eager);
        }
    }
}