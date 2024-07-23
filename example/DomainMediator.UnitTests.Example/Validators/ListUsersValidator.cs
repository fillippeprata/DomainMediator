using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using DomainMediator.ExampleContext.Domain.Entities.Users.Queries;
using DomainMediator.Tests;

namespace DomainMediator.UnitTests.Example.Validators;

[ExcludeFromCodeCoverage]
public class ListUsersValidatorTests
{
    public ListUsersValidatorTests()
    {
        _ = new TestProvider();
    }
    private readonly Fixture _fixture = new();
    private readonly ListUsersValidator validator = new();

    [Fact]
    public void NotEmptyFields()
    {
        // Arrange
        var query = new ListUsersQuery();

        // Act
        var result = validator.Validate(query);

        // Assert
        result.ValidateErrors(
            "'User Name' must not be empty.",
            "'Call As' must not be empty."
        );
    }

    [Fact]
    public void OnSuccess()
    {
        // Arrange
        var query = _fixture.Build<ListUsersQuery>().Create();

        // Act
        var result = validator.Validate(query);

        // Assert
        result.Assert();
    }

    [Fact]
    public void OnlyUserName()
    {
        // Arrange
        var query = new ListUsersQuery { UserName = "query" };

        // Act
        var result = validator.Validate(query);

        // Assert
        result.Assert();
    }

    [Fact]
    public void OnlyCallAs()
    {
        // Arrange
        var query = new ListUsersQuery { CallAs = "query" };

        // Act
        var result = validator.Validate(query);

        // Assert
        result.Assert();
    }
}
