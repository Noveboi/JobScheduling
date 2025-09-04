namespace JobScheduling.Persistence.Domain;

internal sealed class UserJob
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public required Guid UserId { get; init; }
    public required string Key { get; init; }
    public required string Description { get; init; }
}