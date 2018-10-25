using System;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MicroservicesChassis.ServiceDiscovery
{
    public static class ConsulExtensions
    {
        private static readonly string SectionName = "consul";

        public static IServiceCollection AddConsul(this IServiceCollection services)
        {
            IConfiguration config;
            using (var serviceProvider = services.BuildServiceProvider())
            {
               config = serviceProvider.GetService<IConfiguration>();
            }

            var consulOptsSection = config.GetSection(SectionName);
            services.Configure<ConsulOptions>(consulOptsSection);
            services.AddTransient<IConsulServicesRegistry, ConsulServicesRegistry>();

            var options = consulOptsSection.Get<ConsulOptions>();
            services.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
            {
                if (!string.IsNullOrEmpty(options.Url))
                    cfg.Address = new Uri(options.Url);
            }));

            return services;
        }

        public static void UseConsul(this IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var options = scope.ServiceProvider.GetService<IOptions<ConsulOptions>>().Value;
                var enabled = options.Enabled;
                if (!enabled)
                    return;

                var uniqueId = Guid.NewGuid().ToString("N");
                var serviceIdentity = options.ServiceIdentity;
                var serviceName = serviceIdentity.Name;
                var serviceId = $"{serviceName}:{uniqueId}";
                var address = serviceIdentity.Address;
                var port = serviceIdentity.Port;
                var registration = new AgentServiceRegistration
                {
                    Name = serviceName,
                    ID = serviceId,
                    Address = address,
                    Port = port,
                };

                if (options.PingEnabled)
                {
                    var pingEndpoint = string.IsNullOrWhiteSpace(options.PingEndpoint) ? "ping" : options.PingEndpoint;
                    var pingInterval = options.PingInterval <= 0 ? 5 : options.PingInterval;
                    var removeAfterInterval = options.RemoveAfterInterval <= 0 ? 10 : options.RemoveAfterInterval;

                    var check = new AgentServiceCheck
                    {
                        Interval = TimeSpan.FromSeconds(pingInterval),
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(removeAfterInterval),
                        HTTP = $"{address}{(port > 0 ? $":{port}" : string.Empty)}/{pingEndpoint}"
                    };

                    registration.Checks = new[] { check };
                }

                var consulClient = scope.ServiceProvider.GetService<IConsulClient>();
                consulClient.Agent.ServiceRegister(registration);
                appLifetime.ApplicationStopped.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(serviceId);
                });
            }
        }
    }
}
