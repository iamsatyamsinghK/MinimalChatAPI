using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinimalChatAPI.Data;
using MinimalChatAPI.Models.Domain;
using MinimalChatAPI.Models.DTO; 
using MinimalChatAPI.Repositories.Implementation;
using MinimalChatAPI.Repositories.Interface;

namespace MinimalChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext; // Assuming you have a database context named ApplicationDbContext
        private readonly ITokenRepository _tokenRepository;

        public AuthController(ApplicationDbContext dbContext, ITokenRepository tokenRepository)
        {
            _dbContext = dbContext;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            // Check if email is already registered
            if (_dbContext.Users.Any(u => u.Email == model.Email))
            {
                return Conflict("Email is already registered");
            }

            // Hash the password (you can use a library like BCrypt or similar)
            string hashedPassword = HashPassword(model.Password);

            var user = new User
            {
                Email = model.Email,
                Name = model.Name,
                Password = hashedPassword
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var response = new RegisterResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return Ok(response);
        }

        private string HashPassword(string password)
        {
            // Generate a random salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash the password with the salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Email == loginRequest.Email);

            if (user == null || !VerifyPasswordHash(loginRequest.Password, user.Password))
            {
                return Unauthorized();
            }

            // Password is verified, proceed with generating JWT token and returning response

            var jwtToken = _tokenRepository.CreateJwtToken(user);

            var response = new LoginResponseDto
            {
                Token = jwtToken,
                Profile = new UserProfileDto
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email }
            };

            return Ok(response);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
