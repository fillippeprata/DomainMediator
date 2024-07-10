#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.

using MediatR;

namespace DomainMediator.Commands;

public abstract class DomainCommandHandler<CommandT> : INotificationHandler<CommandT> where CommandT : IDomainCommand
{
    public async Task Handle(CommandT notification, CancellationToken cancellationToken)
    {
        await Handle(notification);
    }

    public abstract Task Handle(CommandT command);
}

public abstract class DomainCommandHandler<CommandT, ReturnT> : IRequestHandler<CommandT, ReturnT>
    where ReturnT : ICommandResponse where CommandT : IDomainCommand<ReturnT>
{
    public async Task<ReturnT?> Handle(CommandT request, CancellationToken cancellationToken)
    {
        return await Handle(request);
    }

    public abstract Task<ReturnT?> Handle(CommandT command);
}