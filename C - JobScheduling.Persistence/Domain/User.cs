namespace JobScheduling.Persistence.Domain;

/// <summary>
/// Super simple user model to simulate users in our application.
/// </summary>
internal sealed class User
{
    public Guid Id { get; private init;  } = Guid.CreateVersion7();
    public required string Name { get; set; }
    public required string EmailAddress { get; set; }

    /// <summary>
    /// Each user can have one or more jobs associated with them.
    ///
    /// We persist this information so that we can easily add/update/remove each user's jobs as we see fit.
    /// </summary>
    public List<UserJob> Jobs { get; set; } = [];
}