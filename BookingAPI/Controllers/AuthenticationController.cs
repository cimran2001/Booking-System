using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using BookingAPI.Data;
using BookingAPI.Models;
using BookingDatabaseAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookingAPI.Controllers; 

[ApiController]
public class AuthenticationController : Controller {
    private readonly IConfiguration _configuration;
    
    public AuthenticationController(IConfiguration configuration) {
        _configuration = configuration;
    }

    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Login([FromBody] User user) {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(Routes.UserEndpoints.UserApiUrl);
        httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await httpClient.GetAsync($"?username={user.Username}");
        if (!response.IsSuccessStatusCode)
            return BadRequest();

        var parsedUser = await response.Content.ReadFromJsonAsync<UserResponseModel>();
        if (parsedUser is null || user.Password != parsedUser.PasswordHash)
            return BadRequest();
        
        var claims = new List<Claim> {
            new (ClaimTypes.NameIdentifier, parsedUser.Username),
            new (ClaimTypes.PrimarySid, parsedUser.Id.ToString()),
        };

        foreach (var role in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, role.Name));

        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"]!,
            audience: _configuration["Jwt:Audience"]!,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)), 
                SecurityAlgorithms.HmacSha512)
        );
        
        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    [HttpGet]
    [Authorize]
    [Route("/logincheck")]
    public IActionResult LoginCheck() {
        return Ok("Hello, logged User!");
    }
}