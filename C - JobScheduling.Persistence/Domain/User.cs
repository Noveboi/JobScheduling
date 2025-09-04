namespace JobScheduling.Persistence.Domain;

internal sealed class User
{
    public Guid Id { get; private init;  } = Guid.CreateVersion7();
    public required string Name { get; set; }

    public List<UserJob> Jobs { get; set; } = [];
}