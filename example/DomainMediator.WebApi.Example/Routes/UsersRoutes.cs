using DomainMediator.ExampleContext.Domain.Entities.Users;
using DomainMediator.ExampleContext.Domain.Entities.Users.Commands;
using DomainMediator.ExampleContext.Domain.Entities.Users.Queries;
using DomainMediator.WebApi.WebRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomainMediator.WebApi.Example.Routes;

internal static class UsersRoutes
{
    private static string Route => ContextExampleRoutes.Route("users");
    public static string RouteWithUserId => Route + "/{userId}";

    internal static void AddUsersRoutes(this RouteGroupBuilder app)
    {
        app.AddUser();
        app.ListUsers();
        app.FindUser();
        app.UpdateUser();
        app.DeleteUser();
        app.UpdateUserNotificationSettings();
    }

    private static void AddUser(this RouteGroupBuilder app)
    {
        app.MapPost(Route,
                [AllowAnonymous] async ([FromBody] AddUserCommand command, [FromServices] DomainApi _domainApi) => { return await _domainApi.Exec(command); })
            .Produces<ApiResponse<UserResponse>>(StatusCodes.Status201Created)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x =>
            {
                x.Summary = "Add a new user.";
                return x;
            });
    }

    private static void ListUsers(this RouteGroupBuilder app)
    {
        app.MapGet(Route,
                [AllowAnonymous] async ([AsParameters] ListUsersQuery command, [FromServices] DomainApi _domainApi) => await _domainApi.Get(command))
            .Produces<ApiResponse<UserResponse>>(StatusCodes.Status201Created)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x =>
            {
                x.Summary = "List Users.";
                return x;
            });
    }

    private static void FindUser(this RouteGroupBuilder app)
    {
        app.MapGet(RouteWithUserId, async ([FromRoute] Guid userId, [FromServices] DomainApi _domainApi) =>
            {
                FindUserQuery query = new();
                query.SetUserId(userId);
                return await _domainApi.Get(query);
            })
            .Produces<ApiResponse<UserResponse>>()
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .WithOpenApi(x =>
            {
                x.Summary = "Find a user by userId.";
                return x;
            });
    }

    private static void UpdateUser(this RouteGroupBuilder app)
    {
        app.MapPut(RouteWithUserId, async ([FromRoute] Guid userId, [FromBody] UpdateUserCommand command,
                [FromServices] DomainApi _domainApi) =>
            {
                command.SetUserId(userId);
                return await _domainApi.Exec(command);
            })
            .Produces<ApiResponse>()
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x =>
            {
                x.Summary = "Update user data.";
                return x;
            });
    }

    private static void DeleteUser(this RouteGroupBuilder app)
    {
        app.MapDelete(RouteWithUserId, async ([FromRoute] Guid userId, [FromServices] DomainApi _domainApi) =>
            {
                var command = new DeleteUserCommand();
                command.SetUserId(userId);
                return await _domainApi.Exec(command);
            })
            .Produces<ApiResponse>()
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x =>
            {
                x.Summary = "Update user data.";
                return x;
            });
    }

    private static void UpdateUserNotificationSettings(this RouteGroupBuilder app)
    {
        app.MapPatch($"{RouteWithUserId}/notification-settings", async ([FromRoute] Guid userId,
                [FromBody] UpdateUserNotificationSettingsCommand command, [FromServices] DomainApi _domainApi) =>
            {
                command.SetUserId(userId);
                return await _domainApi.Exec(command);
            })
            .Produces<ApiResponse<UserResponse>>()
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status500InternalServerError)
            .WithOpenApi(x =>
            {
                x.Summary = "Update user data.";
                return x;
            });
    }
}
