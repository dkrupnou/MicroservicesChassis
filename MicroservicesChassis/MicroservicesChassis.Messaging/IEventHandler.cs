using System.Threading.Tasks;

namespace MicroservicesChassis.Messaging
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}