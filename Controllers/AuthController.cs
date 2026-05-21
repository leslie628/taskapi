using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Model;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        // REGISTER
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            var userExists = _context.AppUsers.Any(u => u.Username == dto.Username);

            if (userExists)
                return BadRequest("User already exists");

            var user = new AppUser
            {
                Username = dto.Username,
                PasswordHash = dto.Password // (plain for now - learning stage)
            };

            _context.AppUsers.Add(user);
           await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        // LOGIN
        [HttpPost("login")]
        public ActionResult Login(LoginDto dto)
        {
            var user = _context.AppUsers
                .FirstOrDefault(u => u.Username == dto.Username && u.PasswordHash == dto.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(user);
            Response.Cookies.Append("authToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // important if frontend is on different domain
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok();
        }

        private string GenerateJwtToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
