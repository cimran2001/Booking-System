using BookingDatabaseAPI.Data;
using BookingDatabaseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingDatabaseAPI.Hotels; 

[ApiController]
[Route("[controller]/")]
public class HotelController : Controller {
    private readonly BookingDbContext _context;
    
    public HotelController(BookingDbContext context) {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get() {
        var hotels = await _context.Hotels.ToListAsync();
        return Ok(hotels);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id) {
        var hotel = await _context.Hotels
            .Include(h => h.Apartments)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel is null)
            return NotFound();
        
        return Ok(hotel);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add([FromBody] Hotel hotel) {
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete]
    [Route("remove/{id:int}")]
    public async Task<IActionResult> Remove(int id) {
        var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);

        if (hotel is null)
            return NotFound();
        
        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();

        return Ok();
    }
}