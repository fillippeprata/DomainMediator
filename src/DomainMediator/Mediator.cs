using System.Diagnostics.CodeAnalysis;
using DomainMediator.Commands;
using DomainMediator.Events;
using DomainMediator.Notifications;
using DomainMediator.Queries;
using MediatR;

namespace DomainMediator;

public abstract class Mediator(ScopedNotifications _notifications)
{
    public int GetHttpStatusCode()
    {
        if (Notifications.Count <= 0) return 200; //OK

        if (_notifications.ContainsBadRequestNotification)
            return 400;

        if (_notifications.ContainsNotFoundNotification)
            return 404;

        if (_notifications.ContainsForbiddenNotification)
            return 403;

        if (_notifications.ContainsSystemError)
            return 500;

        return _notifications.ContainsSuccessfullyCreatedNotification ? 201 : 200;
    }

    #region Commands

    public async Task Exec(IDomainCommand command)
    {
        if (Blocked)
            return;

        await ExecImp(command);
    }

    public async Task<CommandResponseT?> Exec<CommandResponseT>(IDomainCommand<CommandResponseT> command)
        where CommandResponseT : ICommandResponse
    {
        if (Blocked)
            return default;

        return await ExecImp(command);
    }

    protected abstract Task ExecImp(IDomainCommand command);

    protected abstract Task<CommandResponseT> ExecImp<CommandResponseT>(IDomainCommand<CommandResponseT> command)
        where CommandResponseT : ICommandResponse;

    #endregion

    #region Queries

    public async Task<QueryResponseT?> Get<QueryResponseT>(IDomainQuery<QueryResponseT> query)
        where QueryResponseT : IQueryResponse
    {
        if (Blocked)
            return default;

        return await GetImp(query);
    }

    protected abstract Task<QueryResponseT> GetImp<QueryResponseT>(IDomainQuery<QueryResponseT> query)
        where QueryResponseT : IQueryResponse;

    #endregion

    #region Events

    public async Task PublishAndWait(IDomainEvent publishedEvent)
    {
        if (Blocked)
            return;

        await PublishImpAndWait(publishedEvent);
    }

    protected abstract Task PublishImpAndWait(IDomainEvent publishedEvent);

    public void Publish(IDomainEvent publishedEvent)
    {
        if (Blocked)
            return;
        PublishImp(publishedEvent);
    }

    protected abstract void PublishImp(IDomainEvent publishedEvent);

    #endregion

    #region Notifications

    [ExcludeFromCodeCoverage] public ScopedNotifications ScopedNotifications => _notifications;

    public IReadOnlyCollection<DomainNotification> Notifications => _notifications.List;

    public bool ContainsNotification(string message) =>_notifications.List.Exists(x => x.Message == message);

    public void AddNotification(DomainNotification notification) => _notifications.Add(notification);

    public void AddNotification(string message, DomainNotificationType notificationType) => _notifications.Add(message, notificationType);

    public void AddNotification(Exception ex) => _notifications.Add(ex);

    public bool ContainsUserNotification => _notifications.ContainsUserNotification;
    public bool NoSystemErrors => _notifications.NoSystemErrors;
    public bool ContainsSystemError => _notifications.ContainsSystemError;
    public bool Blocked => _notifications.Blocked;
    public bool Unblocked => _notifications.Unblocked;

    #endregion
}

internal class MediatorImp(ScopedNotifications _notifications, IMediator _mediator) : Mediator(_notifications)
{
    protected override async Task ExecImp(IDomainCommand command) => await _mediator.Publish(command);

    protected override async Task<CommandReturnT> ExecImp<CommandReturnT>(IDomainCommand<CommandReturnT> command) => await _mediator.Send(command);

    protected override async Task<QueryResponseT> GetImp<QueryResponseT>(IDomainQuery<QueryResponseT> query) => await _mediator.Send(query);

    protected override void PublishImp(IDomainEvent publishedEvent) => new Thread(() => _mediator.Publish(publishedEvent)).Start();

    protected override async Task PublishImpAndWait(IDomainEvent publishedEvent) => await _mediator.Publish(publishedEvent);
}
