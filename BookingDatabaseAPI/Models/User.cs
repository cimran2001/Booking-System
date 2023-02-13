using System.Security.Cryptography;
using System.Text;

namespace BookingDatabaseAPI.Models; 

#nullable disable

public class User {
    public int Id { get; set; }
    public string Username { get; set; }

    private string _passwordHash;
    public string Password {
        get => _passwordHash;
        set => _passwordHash = HashPassword(value);
    }

    public ICollection<Role> Roles { get; set; }

    private static string HashPassword(string password) {
        var data = Encoding.UTF8.GetBytes(password);
        using var algorithm = SHA512.Create();
        var hash = algorithm.ComputeHash(data);
        return Convert.ToBase64String(hash);
    }
}