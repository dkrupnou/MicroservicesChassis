using System.Collections.Generic;
using System.Threading.Tasks;
using MicroservicesChassis.ServiceDiscovery;

namespace MicroservicesChassis.ApiGateway.Clients.Playground
{
    public class PlaygroundClient : IPlaygorundClient
    {
        private readonly IConsulHttpClient _httpClient;

        public PlaygroundClient(IConsulHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            return await _httpClient.GetAsync<IEnumerable<string>>("playground/api/values");
        }
    }
}
