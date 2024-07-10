using DomainMediator.Notifications;
using FluentValidation.Results;

namespace DomainMediator.WebApi.Extensions;

public static class ValidationResultExtensions
{
    public static void AddToMediator(this ValidationResult validationResult, Mediator mediator)
    {
        foreach (var error in validationResult.Errors.Where(error => !mediator.ContainsNotification(error.ErrorMessage)))
            mediator.AddNotification(new DomainNotification
            {
                Message = error.ErrorMessage,
                NotificationTypeEnum = DomainNotificationType.BadRequest,
                Property = error.PropertyName
            });
    }
}
