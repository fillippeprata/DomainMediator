using System.Linq.Expressions;
using AutoMapper;
using DomainMediator.DataBase;
using DomainMediator.ExampleContext.Domain.Entities.Users;
using DomainMediator.ExampleContext.Domain.Entities.Users.Queries;
using DomainMediator.ExampleContext.Domain.Entities.Users.Repository;
using DomainMediator.Notifications;
using Microsoft.EntityFrameworkCore;

namespace DomainMediator.EntityFramework.Example.Users;

internal class UserQuery(IMapper _mapper, Mediator _mediator, ExampleContextDbContext _context) : IUserQuery
{
    public async Task<UserResponse?> FindById(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            _mediator.AddNotification("User not found.", DomainNotificationType.NotFound);
        return _mapper.Map<UserResponse>(user);
    }

    public Task<PageResultResponse<UserResponse>> ListUsers(ListUsersQuery query)
    {
        var users = _context.Users
            .Where(x =>
                (
                    string.IsNullOrEmpty(query.UserName)
                    ||
                    x.UserName.Contains(query.UserName, StringComparison.OrdinalIgnoreCase)
                )
                && (
                    string.IsNullOrEmpty(query.CallAs)
                    ||
                    x.CallAs.Contains(query.CallAs, StringComparison.OrdinalIgnoreCase)
                ))
            .RunPagedQuery<UserEntity, UserResponse>(_mapper, query);
        return Task.FromResult(users);
    }
}
