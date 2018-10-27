using System.Threading.Tasks;

namespace MicroservicesChassis.Messaging
{
    public interface IBusPublisher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;

        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}