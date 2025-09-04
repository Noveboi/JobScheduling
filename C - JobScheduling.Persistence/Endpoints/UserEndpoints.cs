using JobScheduling.Persistence.Application;
using JobScheduling.Persistence.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobScheduling.Persistence.Endpoints;

internal static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.GetUsersGroup();

        group.MapGet("", GetAllUsers);
        group.MapGet("{id:guid}", GetUser);
        group.MapPost("", AddUser);
    }

    public static RouteGroupBuilder GetUsersGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup("users");
    }

    private static async Task<List<User>> GetAllUsers(
        [FromServices] ApplicationDbContext context,
        CancellationToken ct)
    {
        return await context.Users.ToListAsync(ct);
    }

    private static async Task<IResult> GetUser(
        [FromRoute] Guid id,
        [FromServices] ApplicationDbContext context,
        CancellationToken ct)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id, ct) is { } user
            ? Results.Ok(user)
            : Results.NotFound($"User with ID `{id}` does not exist");
    }

    private sealed record AddUserRequest(string Name, string Email);
    private static async Task<IResult> AddUser(
        [FromBody] AddUserRequest req,
        [FromServices] ApplicationDbContext context,
        CancellationToken ct)
    {
        if (await context.Users.AnyAsync(u => u.Name == req.Name, ct))
        {
            return Results.Conflict($"Name '{req.Name}' already exists");
        }

        var user = new User { Name = req.Name, EmailAddress = req.Email };

        context.Users.Add(user);
        await context.SaveChangesAsync(ct);

        return Results.Created($"users/{user.Id}", user);
    }
}