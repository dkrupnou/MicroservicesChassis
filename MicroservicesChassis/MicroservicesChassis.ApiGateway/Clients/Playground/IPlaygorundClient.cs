using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroservicesChassis.ApiGateway.Clients.Playground
{
    public interface IPlaygorundClient
    {
        Task<IEnumerable<string>> GetAsync();
    }
}
