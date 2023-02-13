namespace BookingDatabaseAPI.Models; 

#nullable disable

public class Hotel {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    
    public ICollection<Apartment> Apartments { get; set; }
}
