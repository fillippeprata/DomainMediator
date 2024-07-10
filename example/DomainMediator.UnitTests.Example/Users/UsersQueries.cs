using System.Diagnostics.CodeAnalysis;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.ExampleContext.Domain.Entities.Users.Queries;

namespace DomainMediator.UnitTests.Example.Users;

[ExcludeFromCodeCoverage]
public class UsersQueries
{
    [Fact]
    public async Task FindUserQueryTest()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        var addUser1Response = await mediator.Exec(new AddUserCommand
        {
            CallAs = "find User Test",
            UserName = "find.user.test",
            Password = "password"
        });

        var query = new FindUserQuery();
        query.SetUserId(addUser1Response!.UserId);

        // Act
        var userResponse = await mediator.Get(query);

        // Assert
        Assert.NotNull(userResponse);
        Assert.Equal(userResponse.UserName, addUser1Response.UserName);
        Assert.Equal(userResponse.CallAs, addUser1Response.CallAs);
    }

    [Fact]
    public async Task WhenSendQuery_ThenCheckIfNotFound()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        // Act
        var response = await mediator.Get(new FindUserQuery());

        // Assert
        Assert.Null(response);
        Assert.True(mediator.ContainsNotification("User not found."));
    }

    [Fact]
    public async Task ListUserQueryTest()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;

        await mediator.Exec(new AddUserCommand
        {
            CallAs = "Query User Test",
            UserName = "query.user.test",
            Password = "password"
        });

        await mediator.Exec(new AddUserCommand
        {
            CallAs = "Query User Test 2",
            UserName = "query.user.test 2",
            Password = "password"
        });

        var queryCallAs = new ListUsersQuery
        {
            CallAs = "query"
        };

        // Act
        var usersCallAs = await mediator.Get(queryCallAs);

        // Assert
        Assert.NotNull(usersCallAs);
        Assert.NotEmpty(usersCallAs.Data);
        Assert.Equal(2, usersCallAs.TotalItems);

        //Arrange
        var queryUniqueCallAs = new ListUsersQuery
        {
            CallAs = "query user test 2"
        };

        // Act
        var usersUniqueCallAs = await mediator.Get(queryUniqueCallAs);

        // Assert
        Assert.NotNull(usersUniqueCallAs);
        Assert.NotEmpty(usersUniqueCallAs.Data);
        Assert.Equal(1, usersUniqueCallAs.TotalItems);

        //Arrange
        var queryUserName = new ListUsersQuery
        {
            UserName = "query"
        };

        // Act
        var usersUserName = await mediator.Get(queryUserName);

        // Assert
        Assert.NotNull(usersUserName);
        Assert.NotEmpty(usersUserName.Data);
        Assert.Equal(2, usersUserName.TotalItems);
    }
}