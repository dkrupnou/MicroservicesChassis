using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;

namespace MicroservicesChassis.Messaging
{
    public class RabbitMqBusSubscriber : IBusSubscriber
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _defaultNamespace;

        public RabbitMqBusSubscriber(IApplicationBuilder app)
        {
            _serviceProvider = app.ApplicationServices.GetService<IServiceProvider>();
            _busClient = _serviceProvider.GetService<IBusClient>();
            _defaultNamespace = "";//_serviceProvider.GetService<RabbitMqOptions>().Namespace;
        }

        public IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null)
            where TCommand : ICommand
        {
            _busClient.SubscribeAsync<TCommand>(command =>
            {
                var commandHandler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
                return commandHandler.HandleAsync(command);
            });

            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null)
            where TEvent : IEvent
        {
            _busClient.SubscribeAsync<TEvent>(@event =>
            {
                var eventHandler = _serviceProvider.GetService<IEventHandler<TEvent>>();
                return eventHandler.HandleAsync(@event);
            });

            return this;
        }

        //private string GetQueueName<T>(string @namespace = null, string name = null)
        //{
        //    @namespace = string.IsNullOrWhiteSpace(@namespace)
        //        ? (string.IsNullOrWhiteSpace(_defaultNamespace) ? string.Empty : _defaultNamespace)
        //        : @namespace;

        //    var separatedNamespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

        //    return (string.IsNullOrWhiteSpace(name)
        //        ? $"{Assembly.GetEntryAssembly().GetName().Name}/{separatedNamespace}{typeof(T).Name.Underscore()}"
        //        : $"{name}/{separatedNamespace}{typeof(T).Name.Underscore()}").ToLowerInvariant();
        //}
    }
}