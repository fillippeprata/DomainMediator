using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.Tests;

namespace DomainMediator.UnitTests.Example.Validators;

[ExcludeFromCodeCoverage]
public class UpdateUserNotificationSettingsValidatorTests
{
    private readonly Fixture _fixture = new();
    private readonly UpdateUserNotificationSettingsValidator validator = new();

    public UpdateUserNotificationSettingsValidatorTests()
    {
        _ = new TestProvider();
    }

    [Fact]
    public void NotEmptyFields()
    {
        // Arrange
        var command = new UpdateUserNotificationSettingsCommand();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.ValidateErrors(
            "'Is Email Notification Allowed' must not be empty.",
            "'Is Sms Notification Allowed' must not be empty.",
            "'Is Push Notification Allowed' must not be empty."
        );
    }

    [Fact]
    public void OnSuccess()
    {
        // Arrange
        var command = _fixture.Build<UpdateUserNotificationSettingsCommand>().Create();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.Assert();
    }
}