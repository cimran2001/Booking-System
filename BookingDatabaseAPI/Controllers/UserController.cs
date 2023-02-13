using BookingDatabaseAPI.Data;
using BookingDatabaseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingDatabaseAPI.Controllers; 

[ApiController]
[Route("[controller]")]
public class UserController : Controller {
    private readonly BookingDbContext _context;
    
    public UserController(BookingDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetByUsername([FromQuery] string? username) {
        if (username is null) {
            var users = await _context.Users
                .Include(u => u.Roles)
                .ToListAsync();
            return Ok(users);
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id) {
        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user is null)
            return NotFound();
    
        return Ok(user);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add([FromBody] User user) {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser is not null)
            return Conflict();

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Ok();
    }
}