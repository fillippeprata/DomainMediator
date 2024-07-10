using System.Net;
using DomainMediator.Notifications;
using DomainMediator.Telemetry;
using DomainMediator.WebApi.Extensions;
using DomainMediator.WebApi.WebRequests;

namespace DomainMediator.WebApi.Middlewares;

public static class ErrorMiddleware
{
    public static void UseErrorMiddleware(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            Exception? exception;
            var notifications = new ScopedNotificationsImp();
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                exception = ex;
                HandleErrors();
                Logger();
                await SerializeResponse();
            }

            #region Local methods

            void HandleErrors()
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                notifications.Add(exception);
            }

            void Logger()
            {
                try
                {
                    var userId = context.UserId();
                    var domainLogger = context.RequestServices.GetService(typeof(IDomainLogger)) as IDomainLogger;
                    domainLogger?.Error(exception, context.CorrelationId(), userId);
                }
                catch (Exception logException)
                {
                    notifications.Add(logException);
                }
            }

            async Task SerializeResponse()
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new ApiResponse(notifications));
            }

            #endregion
        });
    }
}
