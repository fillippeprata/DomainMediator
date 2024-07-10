using DomainMediator.WebApi.Jwt;

namespace DomainMediator.WebApi.Extensions;

public static class HttpContextExtensions
{
    private const string CorrelationIdHeaderKey = "correlation-id";

    public static Guid? CorrelationId(this HttpContext httpContext)
    {
        var guid = httpContext.Request.Headers[CorrelationIdHeaderKey];
        return string.IsNullOrEmpty(guid) ? null : Guid.Parse(guid!);
    }

    public static Guid? UserId(this HttpContext httpContext)
    {
        if (!(httpContext.User.Identity?.IsAuthenticated ?? false)) return null;
        var userId = httpContext.User.Claims.FirstOrDefault(x => x.Type == JwtDefaultClaimKeys.userId.ToString());
        if (userId != null)
            return Guid.Parse(userId.Value);
        return null;
    }
}