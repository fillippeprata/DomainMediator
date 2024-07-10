using System.Diagnostics.CodeAnalysis;
using DomainMediator.ExampleContext.Domain.Entities.Users.Repository;
using DomainMediator.Queries;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Queries;

[ExcludeFromCodeCoverage]
public record FindUserQuery : IDomainQuery<UserResponse>
{
    internal Guid UserId { get; private set; }
    public void SetUserId(Guid userId) => UserId = userId;
}

public class FindUserQueryHandler(IUserQuery _query) : DomainQueryHandler<FindUserQuery, UserResponse>
{
    public override async Task<UserResponse?> Handle(FindUserQuery query) => await _query.FindById(query.UserId);
}
