namespace CleanTaskBoard.Domain.Entities.Users;

public class User
{
    public Guid Id { get; private set; }

    public string Username { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    // ΠΟΤΕ plain text password
    public string PasswordHash { get; private set; } = null!;
    public string PasswordSalt { get; private set; } = null!;

    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? LastLoginAtUtc { get; private set; }

    private User() { } // Για EF

    private User(string username, string email, string passwordHash, string passwordSalt)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static User Create(
        string username,
        string email,
        string passwordHash,
        string passwordSalt
    ) => new(username, email, passwordHash, passwordSalt);

    public void UpdateLastLogin()
    {
        LastLoginAtUtc = DateTime.UtcNow;
    }
}
