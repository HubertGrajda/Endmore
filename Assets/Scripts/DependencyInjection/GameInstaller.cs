using System.Collections.Generic;
using Reflex.Core;
using UnityEngine;

namespace Scripts
{
    public class GameInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private List<MonoService> servicesPrefabs;

        private const string SERVICES_KEEPER_NAME = "Services";
        
        public void InstallBindings(ContainerBuilder builder)
        {
            InstallMonoServices(builder);
        }

        private void InstallMonoServices(ContainerBuilder builder)
        {
            var servicesKeeper = new GameObject(SERVICES_KEEPER_NAME);
            DontDestroyOnLoad(servicesKeeper);
            
            foreach (var servicePrefab in servicesPrefabs)
            {
                if (servicePrefab == null) continue;
                
                var serviceInstance = Instantiate(servicePrefab, servicesKeeper.transform);
                serviceInstance.InstallBindings(builder);
            }
        }
    }
}