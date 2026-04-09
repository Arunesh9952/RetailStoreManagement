using Microsoft.AspNetCore.Mvc;
using RetailShop.DTO;
using RetailShop.Models;
using RetailShop.Services;

namespace RetailShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly EmailService _emailService;

        public AuthController(AuthService authService, EmailService emailService)
        {
            this.authService = authService;
            this._emailService = emailService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var user = authService.Login(loginDTO.Username, loginDTO.Password);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            var token = authService.GenerateToken(user);

            _emailService.SendEmail(
                user.Email,
                "Login Successful 🎉",
                $"<h2>Welcome {user.Username}</h2><p>You logged in successfully.</p>"
            );

            return Ok(new
            {
                message = "Login successful",
                token = token
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = "Customer"
            };

            var result = authService.Register(user);

            if (result == "Username already exists.")
                return BadRequest(result);

            _emailService.SendEmail(
                user.Email,
                "Registration Successful 🎉",
                $"<h2>Welcome {user.Username}</h2><p>Your account has been created successfully.</p>"
            );

            return Ok(result);
        }
    }
}