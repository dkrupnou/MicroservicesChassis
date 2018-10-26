using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroservicesChassis.ApiGateway.Clients.Playground.Model;
using MicroservicesChassis.ServiceDiscovery;

namespace MicroservicesChassis.ApiGateway.Clients.Playground
{
    public class PlaygroundClient : IPlaygorundClient
    {
        private static string ServiceEndpoint = "playground";
        private static string GetValuesApiPath = "/api/values";
        private readonly IConsulHttpClient _httpClient;

        public PlaygroundClient(IConsulHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PlaygroundResponse> GetValuesAsync()
        {
            var values = await _httpClient.GetAsync<IEnumerable<string>>(ServiceEndpoint + GetValuesApiPath);
            return new PlaygroundResponse()
            {
                Values = values.ToArray()
            };
        }
    }
}
