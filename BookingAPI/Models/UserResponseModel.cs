using BookingDatabaseAPI.Models;

namespace BookingAPI.Models; 

#nullable disable

public class UserResponseModel {
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<Role> Roles { get; set; }
}