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
        private readonly ILogger<StaffController> _logger;
        public StaffController(IStaffService staffService, IConfiguration configuration, ILogger<StaffController> logger)
        {
            _staffService = staffService;
            _logger = logger;
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
                _logger.LogError("There are no employees in the system.");
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
                _logger.LogError("Employee is not present");
                return NotFound(new { message = "Employee is not present" });
            }

            return Ok(staff);
        }
        [AllowAnonymous]
        [HttpGet("StudentByStaffId/{staffId}", Name = "GetStudentByStaffId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentByStaffId(int staffId)
        {
            var Studens = await _staffService.GetStudentByStaffId(staffId);

            if ( Studens == null)
            {
                _logger.LogError("Employee is not present");
                return NotFound(new { message = "Employee is not present" });
            }

            return Ok(Studens);
        }


        [HttpPut("{id}", Name = "StaffUpdateById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StaffUpdateById(int id, [FromBody] EditStaffDto editStaffDto)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);

            if (staff == null)
            {
                _logger.LogError("Employee is not present");
                return NotFound(new { message = "Employee is not present" });
            }


            staff.Username = editStaffDto.UserName;
            staff.Email = editStaffDto.Email;
            staff.Role = editStaffDto.Role;

            if (!string.IsNullOrEmpty(editStaffDto.Password))
            {
                staff.PasswordHash = BCrypt.Net.BCrypt.HashPassword(editStaffDto.Password);
            }

            await _staffService.UpdateStaffAsync(staff);


            return Ok(new { message = "The employee has been modified successfully." });
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
                _logger.LogError("Employee is not present");
                return NotFound(new { message = "Employee is not present" });
            }

            if (permission == null)
            {
                _logger.LogError("Object cannot be empty");
                return NotFound(new { message = "Object cannot be empty" });
            }


            if (!int.TryParse(permission.permissions, out int result))
            {
                _logger.LogWarning("Input data is wrong");
                return BadRequest(new { message = "Input data is wrong" });
            }

            await _staffService.UpdateRoleAsync(id, result);


            return Ok(new { message = "Employee permission has been modified successfully." });
        }


        [HttpDelete("{id}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Delete(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);

            if (staff == null)
            {
                _logger.LogWarning("Employee is not present");
                return NotFound(new { message = "Employee is not present" });
            }


            await _staffService.DeleteStaffAsync(id);

            return Ok(new { message = "The employee has been successfully deleted." });
        }
    }
}