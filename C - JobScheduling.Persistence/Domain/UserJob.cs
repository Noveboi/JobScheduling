namespace JobScheduling.Persistence.Domain;

/// <summary>
/// Essentially a navigation entity that links one <see cref="User"/> to a Quartz Job.
/// </summary>
internal sealed class UserJob
{
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    
    /// <summary>
    /// Points to the <see cref="User"/> that this entity references.
    /// </summary>
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// The Quartz job key
    /// </summary>
    public required string JobKey { get; init; }

    /// <summary>
    /// A description of what this job does and how often it does it.
    /// </summary>
    public required string Description { get; init; }
}