using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.WebApi.Extensions;

namespace DomainMediator.UnitTests.Example.Validators;

[ExcludeFromCodeCoverage]
public class GeneralValidatorTests
{
    private readonly Fixture _fixture = new();
    private readonly AddUserValidator validator = new();

    [Fact]
    public void NotEmptyFields()
    {
        // Arrange
        var mediator = new TestProvider().Mediator;
        var command = new AddUserCommand();

        // Act
        var result = validator.Validate(command);

        result.AddToMediator(mediator);

        Assert.True(mediator.ContainsNotification("'User Name' must not be empty."));
    }
}