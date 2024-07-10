namespace DomainMediator.WebApi.Middlewares;

public static class CorrelationIdMiddleware
{
    public static void UseCorrelationIdMiddleware(this WebApplication app)
    {
        const string CorrelationIdHeaderKey = "correlation-id";

        app.Use(async (context, next) =>
        {
            var correlationId = Guid.NewGuid().ToString();

            context.Request.Headers.Append(CorrelationIdHeaderKey, correlationId);
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append(CorrelationIdHeaderKey, correlationId);
                return Task.CompletedTask;
            });

            await next.Invoke(context);
        });
    }
}