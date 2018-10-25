using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;

namespace MicroservicesChassis.ServiceDiscovery
{
    public class ConsulServicesRegistry : IConsulServicesRegistry
    {
        private readonly IConsulClient _client;
        private readonly Random _rand = new Random();

        public ConsulServicesRegistry(IConsulClient client)
        {
            _client = client;
        }

        public async Task<AgentService> GetAsync(string serviceName)
        {
            var allServices = await _client.Agent.Services();
            var services = GetRequestedServiceInstances(allServices.Response, serviceName);

            if (!services.Any())
                return null;

            return PickServiceInstance(services);
        }

        private AgentService PickServiceInstance(IList<AgentService> services)
        {
            var randomSelector = _rand.Next(0, services.Count - 1);
            return services[randomSelector];
        }

        private IList<AgentService> GetRequestedServiceInstances(IDictionary<string, AgentService> allServices, string name)
        {
            var relatedServices = allServices?.Where(x => x.Value.Service.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return relatedServices?.Select(x => x.Value).ToList() ?? new List<AgentService>();
        }
    }
}
