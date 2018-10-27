using MicroservicesChassis.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;

namespace MicroservicesChassis.Messaging
{
    public static class RabbitMqExtensions
    {
        private static readonly string SectionName = "rabbitMq";

        public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
            => new RabbitMqBusSubscriber(app);

        public static void AddRabbitMq(this IServiceCollection services)
        {
            RabbitMqOptions options;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<RabbitMqOptions>(configuration.GetSection(SectionName));
                options = configuration.GetOptions<RabbitMqOptions>(SectionName);
            }

            services.Scan(x => x
                .FromEntryAssembly()
                .AddClasses(cls => cls.AssignableTo(typeof(IEventHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            services.Scan(x => x
                .FromEntryAssembly()
                .AddClasses(cls => cls.AssignableTo(typeof(ICommandHandler<>)))
                .AsMatchingInterface()
                .WithTransientLifetime());

            
            services.AddTransient<IBusPublisher, RabbitMqBusPublisher>();
            services.AddRawRabbit();
        }
        
    }
}