using System.Threading.Tasks;
using Consul;

namespace MicroservicesChassis.ServiceDiscovery
{
    public interface IConsulServicesRegistry
    {
        Task<AgentService> GetAsync(string serviceName);
    }
}
