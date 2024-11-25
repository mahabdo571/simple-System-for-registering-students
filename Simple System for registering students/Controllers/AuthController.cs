using Microsoft.AspNetCore.Mvc;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Services.Interface;


namespace Simple_System_for_registering_students.Controllers
{
    /// <summary>
    /// Controller responsible for handling user authentication and staff registration operations.
    /// </summary>
    //[Route("api/[controller]")]
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IStaffService _staffService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Constructor for AuthController.
        /// </summary>
        /// <param name="authService">Service for authentication-related operations.</param>
        /// <param name="staffService">Service for staff management operations.</param>
        public AuthController(IAuthService authService, IStaffService staffService, ILogger<AuthController> logger)
        {
            _authService = authService;           
            _staffService = staffService;
            _logger = logger;
        }

        /// <summary>
        /// Handles user registration.
        /// </summary>
        /// <param name="registerModel">Data Transfer Object containing registration details.</param>
        /// <returns>
        /// - 200 OK if registration is successful.
        /// - 400 Bad Request if the input data is invalid.
        /// - 409 Conflict if the email is already in use.
        /// </returns>
        [HttpPost("register",Name = "Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerModel)
        {
            if (registerModel == null)
            {
                _logger.LogError("Input data is missing");

                return BadRequest(new { message = "Input data is missing" });
            }

            if (string.IsNullOrEmpty(registerModel.Email) || string.IsNullOrEmpty(registerModel.Password)|| string.IsNullOrEmpty(registerModel.ConfirmPassword))
            {
                _logger.LogError("Email and Password are required");

                return BadRequest(new { message = "Email and Password are required" });
            }

            if (!registerModel.Password.Equals(registerModel.ConfirmPassword))
            {
                _logger.LogWarning("Pass Not Match");
                return BadRequest(new { message = "Pass Not Match" });
            }
      
            var existingUser = await _staffService.GetByEmailAsync(registerModel.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Email is already in use");
                return Conflict(new { message = "Email is already in use" });
            }

       
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);

            var staff = new Staff
            {
                Email = registerModel.Email,
                PasswordHash = hashedPassword,
                Username = registerModel.UserName,
                Role=1

            };

            await _staffService.RegisterStaffAsync(staff);
      


            return Ok();
        }


        /// <summary>
        /// Handles user login.
        /// </summary>
        /// <param name="loginModel">Data Transfer Object containing login details.</param>
        /// <returns>
        /// - 200 OK if login is successful, along with a JWT token.
        /// - 400 Bad Request if the input data is invalid.
        /// - 401 Unauthorized if the credentials are incorrect.
        /// </returns>
        [HttpPost("login",Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginModel)
        {
            if (loginModel == null)
            {
                _logger.LogError("Login data is missing");
                return BadRequest(new { message = "Login data is missing" });
            }

            if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                _logger.LogWarning("Email and Password are required");
                return BadRequest(new { message = "Email and Password are required" });
            }

            var staff = await _authService.AuthenticateAsync(loginModel.Email, loginModel.Password);

            if (staff == null)
            {
                _logger.LogWarning("Invalid email or password");
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = _authService.GenerateJwtToken(staff);

           
            return Ok(new { message = "Login successful" , token  });
        }

     
    }

}
