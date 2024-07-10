using DomainMediator.ExampleContext.Domain.Entities.Users;
using DomainMediator.WebApi.Filters;
using DomainMediator.WebApi.WebRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomainMediator.WebApi.Example.Routes;

public static class ContextExampleRoutes
{
    public static string DefaultVersion => "v1";

    public static string Route(string route)
    {
        return $"{DefaultVersion}/example/{route}";
    }

    public static void AddContextExampleRoutes(this WebApplication app)
    {
        var global = app.MapGroup(string.Empty)
            .WithAutoValidation();

        global.AddAuthRoutes();
        global.AddUsersRoutes();
        global.TestErrorMiddleware();
    }

    private static void TestErrorMiddleware(this IEndpointRouteBuilder app)
    {
        app.MapGet(Route("error"),
                ([FromServices] DomainApi domainApi) => { throw new Exception("Test Error Middleware."); } )
            .Produces<ApiResponse<UserResponse>>(StatusCodes.Status500InternalServerError);
    }
}
