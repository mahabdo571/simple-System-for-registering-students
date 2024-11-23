using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Repositories;
using Simple_System_for_registering_students.Services;
using Simple_System_for_registering_students.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


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
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
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
                Password = hashedPassword,
                Name = registerModel.Name
            };

            await _staffService.RegisterStaffAsync(staff);

          
            return Ok();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
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
