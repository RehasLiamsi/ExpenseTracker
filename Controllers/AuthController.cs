using ExpenseTracker.Data;
using ExpenseTracker.DTO;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        public AuthController(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthDTO authDTO)
        {
            if (await _context.Users.AnyAsync(u => u.Email == authDTO.Email))
            {
                return BadRequest(new { message = "Email already in user" });
            }

            var user = new User
            {
                Username = authDTO.Email.Split('@')[0],
                Email = authDTO.Email,
                PasswordHash = _authService.HashPassword(authDTO.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDTO authDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == authDTO.Email);
            if (user == null || !_authService.VerifyPassword(authDTO.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _authService.GenerateJwtToken(user.Id, user.Username);
            return Ok(new { token });
        }
    }
}
