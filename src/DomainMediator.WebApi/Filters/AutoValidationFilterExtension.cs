using System.IO.Pipelines;
using System.Reflection;
using System.Security.Claims;
using DomainMediator.WebApi.Extensions;
using DomainMediator.WebApi.WebRequests;
using FluentValidation;

namespace DomainMediator.WebApi.Filters;

public static class AutoValidationFilterExtension
{
    public static TBuilder WithAutoValidation<TBuilder>(this TBuilder endpoint)
        where TBuilder : IEndpointConventionBuilder
    {
        endpoint.Add(builder =>
        {
            var methodInfo = builder.Metadata.OfType<MethodInfo>().FirstOrDefault();
            if (methodInfo is null || !IsValidatable(methodInfo))
                return;

            builder.FilterFactories.Add((_, next) =>
            {
                return async endpointFilterContext =>
                {
                    if (endpointFilterContext.HttpContext.RequestServices.GetService(typeof(DomainApi)) is not DomainApi _domainApi) return await next(endpointFilterContext);

                    foreach (var argument in endpointFilterContext.Arguments)
                    {
                        if (argument is null || !IsValidatable(argument.GetType())) continue;

                        var service =
                            endpointFilterContext.HttpContext.RequestServices.GetService(
                                typeof(IValidator<>).MakeGenericType(argument.GetType()));
                        if (service is IValidator validator)
                            (await validator.ValidateAsync(new ValidationContext<object>(argument)))
                                .AddToMediator(_domainApi.Mediator);
                    }

                    if (_domainApi.Mediator.Blocked)
                        return _domainApi.Response();

                    return await next(endpointFilterContext);
                };
            });
        });

        return endpoint;
    }

    private static bool IsValidatable(MethodInfo methodInfo) => methodInfo.GetParameters().Any(p => IsValidatable(p.ParameterType));

    private static bool IsValidatable(Type type)
    {
        return typeof(HttpContext) != type
               && typeof(HttpRequest) != type
               && typeof(HttpResponse) != type
               && typeof(ClaimsPrincipal) != type
               && typeof(CancellationToken) != type
               && typeof(IFormFileCollection) != type
               && typeof(IFormFile) != type
               && typeof(Stream) != type
               && typeof(PipeReader) != type
               && !IsNotACustomType(type);
    }

    private static bool IsNotACustomType(Type type)
    {
        var builtInTypes = new[]
        {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Enum)
        };
        return builtInTypes.Contains(type);
    }
}
