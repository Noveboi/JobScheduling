namespace JobScheduling.Persistence.Endpoints;

internal static class JobEndpoints
{
    public static void MapJobEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.GetUsersGroup();
    }
}