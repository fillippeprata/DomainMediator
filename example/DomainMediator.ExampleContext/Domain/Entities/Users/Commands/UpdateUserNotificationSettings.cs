using System.Diagnostics.CodeAnalysis;
using DomainMediator.Commands;
using FluentValidation;

namespace DomainMediator.ExampleContext.Domain.Entities.Users.Commands;

[ExcludeFromCodeCoverage]
public record UpdateUserNotificationSettingsCommand : IDomainCommand
{
    internal Guid UserId { get; private set; }
    public bool? IsEmailNotificationAllowed { get; init; }
    public bool? IsSmsNotificationAllowed { get; init; }
    public bool? IsPushNotificationAllowed { get; init; }
    public void SetUserId(Guid userId) => UserId = userId;
}

public class UpdateUserNotificationSettingsValidator : AbstractValidator<UpdateUserNotificationSettingsCommand>
{
    public UpdateUserNotificationSettingsValidator()
    {
        RuleFor(x => x.IsEmailNotificationAllowed).NotNull();
        RuleFor(x => x.IsSmsNotificationAllowed).NotNull();
        RuleFor(x => x.IsPushNotificationAllowed).NotNull();
    }
}

public class UpdateUserNotificationSettingsCommandHandler(UserFactory _factory)
    : DomainCommandHandler<UpdateUserNotificationSettingsCommand>
{
    public override async Task Handle(UpdateUserNotificationSettingsCommand command)
    {
        var user = await _factory.FindUserAsync(command.UserId);
        if (user == null) return;
        await user.UpdateNotificationSettingsAndSave(command);
    }
}
