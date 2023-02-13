namespace BookingDatabaseAPI.Models; 

#nullable disable

public class Apartment {
    public int Id { get; set; }
    public string Name { get; set; }
    public int RoomsCount { get; set; }
    public decimal Price { get; set; }
    
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
}