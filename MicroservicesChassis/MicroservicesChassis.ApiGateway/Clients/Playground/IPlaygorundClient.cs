using System.Collections.Generic;
using System.Threading.Tasks;
using MicroservicesChassis.ApiGateway.Clients.Playground.Model;

namespace MicroservicesChassis.ApiGateway.Clients.Playground
{
    public interface IPlaygorundClient
    {
        Task<PlaygroundResponse> GetValuesAsync();
    }
}
