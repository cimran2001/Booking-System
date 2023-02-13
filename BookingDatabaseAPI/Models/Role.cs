namespace BookingDatabaseAPI.Models; 

#nullable disable

public class Role {
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<User> Users { get; set; }
}