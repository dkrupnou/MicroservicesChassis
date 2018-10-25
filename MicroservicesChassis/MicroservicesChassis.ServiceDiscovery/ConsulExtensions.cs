using System;
using System.Linq;
using Consul;
using MicroservicesChassis.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
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

            services.Configure<ConsulOptions>(config.GetSection(SectionName));
            services.AddTransient<IConsulServicesRegistry, ConsulServicesRegistry>();

            var options = config.GetOptions<ConsulOptions>(SectionName);
            services.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
            {
                if (!string.IsNullOrEmpty(options.Url))
                    cfg.Address = new Uri(options.Url);
            }));

            return services;
        }

        public static void UseConsul(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var options = scope.ServiceProvider.GetService<IOptions<ConsulOptions>>().Value;
                var enabled = options.Enabled;
                if (!enabled)
                    return;

                var registration = CreateRegistration(options.ServiceIdentity);

                if (options.PingEnabled)
                {
                    var pingEndpoint = string.IsNullOrWhiteSpace(options.PingEndpoint) ? "ping" : options.PingEndpoint;
                    var pingInterval = options.PingInterval <= 0 ? 5 : options.PingInterval;
                    var removeAfterInterval = options.RemoveAfterInterval <= 0 ? 10 : options.RemoveAfterInterval;

                    var check = new AgentServiceCheck
                    {
                        Interval = TimeSpan.FromSeconds(pingInterval),
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(removeAfterInterval),
                        HTTP = $"{registration.Address}{(registration.Port > 0 ? $":{registration.Port}" : string.Empty)}/{pingEndpoint}"
                    };

                    registration.Checks = new[] { check };
                }

                var consulClient = scope.ServiceProvider.GetService<IConsulClient>();
                consulClient.Agent.ServiceRegister(registration);

                var appLifetime = scope.ServiceProvider.GetService<IApplicationLifetime>();
                appLifetime.ApplicationStopped.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(registration.ID);
                });
            }
        }

        private static AgentServiceRegistration CreateRegistration(ServiceIdentity identity)
        {
            var uniqueId = Guid.NewGuid().ToString("N");
            var serviceName = identity.Name;
            var serviceId = $"{serviceName}:{uniqueId}";

            return new AgentServiceRegistration
            {
                ID = serviceId,
                Name = serviceName,
                Address = identity.Address,
                Port = identity.Port
            };
        }
    }
}
