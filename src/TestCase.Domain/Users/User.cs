using TestCase.Domain.Abstractions;
using TestCase.Domain.Users.ValueObjects;

namespace TestCase.Domain.Users;

public sealed class User : Entity
{
    public string UserName { get; set; } = default!;
    public Password Password { get; set; } = default!;
    
    public bool VerifyPasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Password.PasswordHash);
    }

}