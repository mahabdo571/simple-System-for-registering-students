using Microsoft.AspNetCore.Mvc;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Services.Interface;


namespace Simple_System_for_registering_students.Controllers
{

    //[Route("api/[controller]")]
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IStaffService _staffService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(IAuthService authService, IStaffService staffService, ILogger<AuthController> logger)
        {
            _authService = authService;
           
            _staffService = staffService;
            _logger = logger;
        }

  
        [HttpPost("register",Name = "Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerModel)
        {
            if (registerModel == null)
            {
                _logger.LogWarning("Input data is missing");

                return BadRequest(new { message = "Input data is missing" });
            }

            if (string.IsNullOrEmpty(registerModel.Email) || string.IsNullOrEmpty(registerModel.Password)|| string.IsNullOrEmpty(registerModel.ConfirmPassword))
            {
                return BadRequest(new { message = "Email and Password are required" });
            }

            if (!registerModel.Password.Equals(registerModel.ConfirmPassword))
            {
                return BadRequest(new { message = "Pass Not Match" });
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
                
                



            };

            await _staffService.RegisterStaffAsync(staff);

        
            return Ok();
        }


        [HttpPost("login",Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

            _logger.LogInformation("Login successful log");
            return Ok(new { message = "Login successful" , token  });
        }

     
    }

}
