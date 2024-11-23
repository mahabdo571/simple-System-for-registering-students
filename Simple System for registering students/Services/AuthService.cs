using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Simple_System_for_registering_students.Services
{
   public class AuthService : IAuthService
    {
      
        private readonly IStaffService _staffService;
        private readonly IConfiguration _configuration;

        public AuthService(IStaffService staffService, IConfiguration configuration)
        {
            _staffService = staffService;
            _configuration = configuration;
        }

        public async Task<Staff> AuthenticateAsync(string email, string password)
        {
            var staff = await _staffService.GetByEmailAsync(email);

            if (staff == null || !BCrypt.Net.BCrypt.Verify(password, staff.PasswordHash))
            {
                return null; 
            }

            return staff;
        }

        public string GenerateJwtToken(Staff staff)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, staff.Id.ToString()),
                new Claim(ClaimTypes.Name, staff.Username),
                new Claim(ClaimTypes.Email, staff.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

}
