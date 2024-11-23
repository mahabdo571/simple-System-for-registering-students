using Microsoft.AspNetCore.Mvc;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Services.Interface;


namespace Simple_System_for_registering_students.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IStaffService _staffService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration, IStaffService staffService)
        {
            _authService = authService;
            _configuration = configuration;
            _staffService = staffService;
        }

  
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerModel)
        {
            if (registerModel == null)
            {
                return BadRequest(new { message = "Input data is missing" });
            }

            if (string.IsNullOrEmpty(registerModel.Email) || string.IsNullOrEmpty(registerModel.Password))
            {
                return BadRequest(new { message = "Email and Password are required" });
            }

      
            var existingUser = await _staffService.GetByEmailAsync(registerModel.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email is already in use" });
            }

       
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);

            var staff = new Staff
            {
                Email = registerModel.Email,
                PasswordHash = hashedPassword,
                Username = registerModel.UserName,
                Role = "-1",
                



            };

            await _staffService.RegisterStaffAsync(staff);

          
            return Ok();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest(new { message = "Login data is missing" });
            }

            if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new { message = "Email and Password are required" });
            }

            var staff = await _authService.AuthenticateAsync(loginModel.Email, loginModel.Password);

            if (staff == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = _authService.GenerateJwtToken(staff);


            return Ok(new { message = "Login successful" , token  });
        }

     
    }

}
