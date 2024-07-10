using DomainMediator.ExampleContext.Domain.Entities.Users;
using DomainMediator.ExampleContext.Domain.Entities.Users.Repository;

namespace DomainMediator.ExampleContext.Domain.Services;

public interface IAuthenticationService
{
    Task<IUserProperties?> AuthenticateAsync(string username, string password);
}

public class AuthenticationServiceImp(IUserStorage _storage) : IAuthenticationService
{
    public async Task<IUserProperties?> AuthenticateAsync(string username, string password)
    {
        var user = await _storage.GetByUserName(username);
        return user?.Password == password ? user : null;
    }
}