using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Simple_System_for_registering_students.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        private readonly IConfiguration _configuration;

        public StaffController(IStaffService staffService,  IConfiguration configuration)
        {
            _staffService = staffService;

            _configuration = configuration;
        }

    
 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var staffList = await _staffService.GetAllStaffsAsync();

            if (staffList.Count() == 0)
            {
                return NotFound(new { message = "لا توجد موظفين في النظام" });
            }

            return Ok(staffList);
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);

            if (staff == null)
            {
                return NotFound(new { message = "الموظف غير موجود" });
            }

            return Ok(staff);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RegisterDto registerDto)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);

            if (staff == null)
            {
                return NotFound(new { message = "الموظف غير موجود" });
            }

           
            staff.Name = registerDto.Name;
            staff.Email = registerDto.Email;
            if (!string.IsNullOrEmpty(registerDto.Password))
            {
                staff.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password); 
            }

            await _staffService.UpdateStaffAsync(staff);
         

            return Ok(new { message = "تم تعديل الموظف بنجاح" });
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);

            if (staff == null)
            {
                return NotFound(new { message = "الموظف غير موجود" });
            }

          
            await _staffService.DeleteStaffAsync(id);

            return Ok(new { message = "تم حذف الموظف بنجاح" });
        }
    }
}