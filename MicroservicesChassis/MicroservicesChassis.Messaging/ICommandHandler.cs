using System.Threading.Tasks;

namespace MicroservicesChassis.Messaging
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}