namespace MicroservicesChassis.Messaging
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null)
            where TCommand : ICommand;

        IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null)
            where TEvent : IEvent;
    }
}