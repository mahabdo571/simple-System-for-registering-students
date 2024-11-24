using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Simple_System_for_registering_students.DTOs;

using Simple_System_for_registering_students.Services.Interface;


namespace Simple_System_for_registering_students.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        private readonly IConfiguration _configuration;

        public StaffController(IStaffService staffService, IConfiguration configuration)
        {
            _staffService = staffService;

            _configuration = configuration;
        }



        [HttpGet("All", Name = "GetAllStaffs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllStaffs()
        {
            var staffList = await _staffService.GetAllStaffsAsync();

            if (staffList.Count() == 0)
            {
                return NotFound(new { message = "There are no employees in the system." });
            }

            return Ok(staffList);
        }


        [HttpGet("{id}", Name = "GetStaffById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStaffById(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);

            if (staff == null)
            {
                return NotFound(new { message = "Employee is not present" });
            }

            return Ok(staff);
        }


        [HttpPut("{id}", Name = "StaffUpdateById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StaffUpdateById(int id, [FromBody] EditStaffDto editStaffDto)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);

            if (staff == null)
            {
                return NotFound(new { message = "الموظف غير موجود" });
            }


            staff.Username = editStaffDto.UserName;
            staff.Email = editStaffDto.Email;
            staff.Role = editStaffDto.Role;

            if (!string.IsNullOrEmpty(editStaffDto.Password))
            {
                staff.PasswordHash = BCrypt.Net.BCrypt.HashPassword(editStaffDto.Password);
            }

            await _staffService.UpdateStaffAsync(staff);


            return Ok(new { message = "تم تعديل الموظف بنجاح" });
        }




        [HttpPatch("{id}", Name = "UpdateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] PermissionDto permission)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);

            if (staff == null)
            {
                return NotFound(new { message = "الموظف غير موجود" });
            }

            if (permission == null)
            {
                return NotFound(new { message = "error" });
            }


            if (!int.TryParse(permission.permissions, out int result))
            {
                return BadRequest(new { message = "Input data is wrong" });
            }

            await _staffService.UpdateRoleAsync(id, result);


            return Ok(new { message = "تم تعديل صلاحية الموظف بنجاح" });
        }


        [HttpDelete("{id}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

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