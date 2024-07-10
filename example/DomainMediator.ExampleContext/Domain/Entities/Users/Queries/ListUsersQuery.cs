using System.Diagnostics.CodeAnalysis;
using DomainMediator.DataBase;
using DomainMediator.ExampleContext.Domain.Entities.Users.Repository;
using DomainMediator.Queries;
using FluentValidation;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Queries;

[ExcludeFromCodeCoverage]
public record ListUsersQuery : PageQueryRequest, IDomainQuery<ListUsersResponse>
{
    public string? UserName { get; init; }
    public string? CallAs { get; init; }
}

public class ListUsersValidator : AbstractValidator<ListUsersQuery>
{
    public ListUsersValidator()
    {
        RuleFor(m => m.UserName).NotEmpty().When(m => string.IsNullOrEmpty(m.CallAs));
        RuleFor(m => m.CallAs).NotEmpty().When(m => string.IsNullOrEmpty(m.UserName));
    }
}

[ExcludeFromCodeCoverage]
public record ListUsersResponse : PageResultResponse<UserResponse>, IQueryResponse
{
    public ListUsersResponse(){}
    public ListUsersResponse(PageResultResponse<UserResponse> result) : base(result){}
}

public class ListUsersQueryHandler(IUserQuery _query) : DomainQueryHandler<ListUsersQuery, ListUsersResponse>
{
    public override async Task<ListUsersResponse?> Handle(ListUsersQuery query)
    {
        var users = await _query.ListUsers(query);
        return new ListUsersResponse(users);
    }
}
