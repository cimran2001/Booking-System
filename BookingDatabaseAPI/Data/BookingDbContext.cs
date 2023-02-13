using BookingDatabaseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingDatabaseAPI.Data; 

#nullable disable

public class BookingDbContext : DbContext {
    public BookingDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Hotel>(h => {
            h.HasMany(h => h.Apartments)
                .WithOne(a => a.Hotel)
                .HasForeignKey(a => a.HotelId);
        });

        modelBuilder.Entity<User>(u => {
            u.HasIndex(u => u.Username)
                .IsUnique();

            u.HasMany(u => u.Roles)
                .WithMany(r => r.Users);
        });

        modelBuilder.Entity<Role>(r => {
            r.HasIndex(r => r.Name)
                .IsUnique();

            r.HasMany(r => r.Users)
                .WithMany(u => u.Roles);
        });
    }
}