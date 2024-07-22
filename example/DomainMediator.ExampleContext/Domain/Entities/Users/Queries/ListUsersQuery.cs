using System.Diagnostics.CodeAnalysis;
using DomainMediator.DataBase;
using DomainMediator.ExampleContext.Domain.Entities.Users.Repository;
using DomainMediator.Queries;
using FluentValidation;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Queries;

[ExcludeFromCodeCoverage]
public record ListUsersQuery : PageQueryRequest, IDomainQuery<PageResultResponse<UserResponse>>
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

public class ListUsersQueryHandler(IUserQuery _query) : DomainQueryHandler<ListUsersQuery, PageResultResponse<UserResponse>>
{
    public override async Task<PageResultResponse<UserResponse>?> Handle(ListUsersQuery query)
    {
        var users = await _query.ListUsers(query);
        return new PageResultResponse<UserResponse>(users);
    }
}
