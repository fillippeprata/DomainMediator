using DomainMediator.WebApi.Example.Features.Auth;
using DomainMediator.WebApi.Jwt;
using DomainMediator.WebApi.WebRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomainMediator.WebApi.Example.Routes;

internal static class AuthRoutes
{
    private static string Route => ContextExampleRoutes.Route("auth");

    internal static void AddAuthRoutes(this RouteGroupBuilder app)
    {
        app.Auth();
    }

    private static void Auth(this RouteGroupBuilder app)
    {
        app.MapPost(Route,
                [AllowAnonymous] async ([FromBody] AuthenticateUserCommand command, [FromServices] DomainApi _domainApi) => { return await _domainApi.Exec(command); })
            .Produces<ApiResponse<JwtResponse>>()
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x =>
            {
                x.Summary = "Authenticate a user.";
                return x;
            });
    }
}