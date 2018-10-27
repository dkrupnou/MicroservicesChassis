using System.Threading.Tasks;
using RawRabbit;

namespace MicroservicesChassis.Messaging
{
    public class RabbitMqBusPublisher : IBusPublisher
    {
        private readonly IBusClient _busClient;

        public RabbitMqBusPublisher(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public async Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand
            => await _busClient.PublishAsync(command);

        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent
            => await _busClient.PublishAsync(@event);
    }
}
