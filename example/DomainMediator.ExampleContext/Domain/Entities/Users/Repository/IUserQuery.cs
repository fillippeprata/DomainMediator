using DomainMediator.DataBase;
using DomainMediator.ExampleContext.Domain.Entities.Users.Queries;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Repository;

public interface IUserQuery
{
    Task<UserResponse?> FindById(Guid id);
    Task<PageResultResponse<UserResponse>> ListUsers(ListUsersQuery query);
}
