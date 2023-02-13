using BookingDatabaseAPI.Data;
using BookingDatabaseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingDatabaseAPI.Hotels; 


[ApiController]
[Route("[controller]")]
public class ApartmentController : Controller {
    private readonly BookingDbContext _context;
    
    public ApartmentController(BookingDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get() {
        var apartments = await _context.Apartments.ToListAsync();

        return Ok();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id) {
        var apartment = await _context.Apartments
            .Include(a => a.Hotel)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (apartment is null)
            return NotFound();

        return Ok(apartment);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add([FromBody] Apartment apartment) {
        await _context.Apartments.AddAsync(apartment);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet]
    [Route("remove/{id:int}")]
    public async Task<IActionResult> Remove(int id) {
        var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == id);

        if (apartment is null)
            return NotFound();

        _context.Apartments.Remove(apartment);
        await _context.SaveChangesAsync();

        return Ok();
    }
}