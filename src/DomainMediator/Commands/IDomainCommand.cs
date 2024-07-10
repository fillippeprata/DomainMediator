using MediatR;

namespace DomainMediator.Commands;

public interface IDomainCommand : INotification
{
}

public interface IDomainCommand<out ResponseT> : IRequest<ResponseT> where ResponseT : ICommandResponse
{
}