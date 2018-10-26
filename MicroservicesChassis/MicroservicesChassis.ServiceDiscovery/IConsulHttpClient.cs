using System.Threading.Tasks;

namespace MicroservicesChassis.ServiceDiscovery
{
    public interface IConsulHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}
