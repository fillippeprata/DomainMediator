using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.Tests;

namespace DomainMediator.UnitTests.Example.Validators;

[ExcludeFromCodeCoverage]
public class AddUserValidatorTests
{
    private readonly Fixture _fixture = new();
    private readonly AddUserValidator _validator = new();

    public AddUserValidatorTests()
    {
        _ = new TestProvider();
    }

    [Fact]
    public void NotEmptyFields()
    {
        // Arrange
        var command = new AddUserCommand();

        // Act
        var result = _validator.Validate(command);

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
        var command = _fixture.Build<AddUserCommand>().Create();

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Assert();
    }
}