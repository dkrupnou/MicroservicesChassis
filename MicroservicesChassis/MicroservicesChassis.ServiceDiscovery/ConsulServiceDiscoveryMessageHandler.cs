using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Polly;

namespace MicroservicesChassis.ServiceDiscovery
{
    public class ConsulServiceDiscoveryMessageHandler : DelegatingHandler
    {
        private readonly IConsulServicesRegistry _consulRegistry;
        private readonly ConsulOptions _options;

        private int RequestRetries => _options.RequestRetries <= 0 ? 3 : _options.RequestRetries;

        public ConsulServiceDiscoveryMessageHandler(IConsulServicesRegistry consulRegistry, IOptions<ConsulOptions> options)
        {
            _consulRegistry = consulRegistry;
            _options = options.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uri = request.RequestUri;
            var serviceName = uri.Host;

            return await Policy.Handle<Exception>()
                .WaitAndRetryAsync(RequestRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () =>
                {
                    request.RequestUri = await GetDirectServiceUriAsync(serviceName, uri);
                    return await base.SendAsync(request, cancellationToken);
                });
        }

        private async Task<Uri> GetDirectServiceUriAsync(string serviceName, Uri uri)
        {
            var service = await _consulRegistry.GetAsync(serviceName);
            if (service == null)
                throw new Exception($"Consul service: '{serviceName}' was not found.");

            var directServiceUri = new Uri($"{service.Address}:{service.Port}");
            var uriBuilder = new UriBuilder(uri)
            {
                Scheme = directServiceUri.Scheme,
                Host = directServiceUri.Host,
                Port = directServiceUri.Port
            };

            return uriBuilder.Uri;
        }
    }
}