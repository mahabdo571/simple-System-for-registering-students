using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Simple_System_for_registering_students.DTOs;

using Simple_System_for_registering_students.Services.Interface;


namespace Simple_System_for_registering_students.Controllers
{   
    /// <summary>
    /// Controller responsible for handling staff-related operations such as 
    /// fetching staff details, updating staff information, and managing staff roles.
    /// It requires authentication for access to its methods, except for the 
    /// "GetStudentByStaffId" endpoint which is accessible to everyone.
    /// </summary>
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/Staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        /// <summary>
        /// The service responsible for managing staff data and business logic.
        /// </summary>
        private readonly IStaffService _staffService;

        /// <summary>
        /// The configuration object to access app settings.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Logger instance to log messages related to the controller's actions.
        /// </summary>
        private readonly ILogger<StaffController> _logger;

        /// <summary>
        /// Constructor to initialize the service, configuration, and logger instances.
        /// </summary>
        /// <param name="staffService">The service for handling staff data</param>
        /// <param name="configuration">App configuration settings</param>
        /// <param name="logger">Logger instance for logging controller actions</param>
        public StaffController(IStaffService staffService, IConfiguration configuration, ILogger<StaffController> logger)
        {
            _staffService = staffService;
            _logger = logger;
            _configuration = configuration;
        }


        /// <summary>
        /// Retrieves the list of all staff members.
        /// </summary>
        /// <returns>A list of all staff members or a not found message if no staff are found.</returns>
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

        /// <summary>
        /// Retrieves a specific staff member by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the staff member</param>
        /// <returns>The staff member details or a not found message if the staff is not found.</returns>
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

        /// <summary>
        /// Retrieves a list of students assigned to a particular staff member, 
        /// this endpoint is accessible to anyone without authentication.
        /// </summary>
        /// <param name="staffId">The unique ID of the staff member</param>
        /// <returns>A list of students or a not found message if no students are associated with the staff member.</returns>
        [HttpGet("StudentByStaffId/{staffId}", Name = "GetStudentByStaffId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetStudentByStaffId(int staffId)
        {
            try
            {
                var Studens = await _staffService.GetStudentByStaffId(staffId);

                if (Studens == null)
                {
                    _logger.LogError("Employee is not present");
                    return NotFound(new { message = "Employee is not present" });
                }

                return Ok(Studens);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return Problem(
            detail: ex.Message,
            statusCode: 403,
            title: "Forbidden"
        );
            }
        }

        /// <summary>
        /// Updates the details of an existing staff member.
        /// </summary>
        /// <param name="id">The unique ID of the staff member to be updated</param>
        /// <param name="editStaffDto">The data transfer object containing updated staff details</param>
        /// <returns>A success message if the staff member is updated successfully, or a not found message if the staff member is not found.</returns>
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
            staff.UpdatedAt = DateTime.Now;



            if (!string.IsNullOrEmpty(editStaffDto.Password))
            {
                staff.PasswordHash = BCrypt.Net.BCrypt.HashPassword(editStaffDto.Password);
            }



            try
            {
                await _staffService.UpdateStaffAsync(staff);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return Problem(
            detail: ex.Message,
            statusCode: 403,
            title: "Forbidden"
        );
            }

            return Ok(new { message = "The employee has been modified successfully." });
        }



        /// <summary>
        /// Updates the role of a staff member.
        /// </summary>
        /// <param name="id">The unique ID of the staff member</param>
        /// <param name="permission">The data transfer object containing the new permissions for the staff member</param>
        /// <returns>A success message if the role is updated successfully, or error messages if the input is invalid or if the staff is not found.</returns>
        [HttpPatch("{id}", Name = "UpdateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

            try
            {
                await _staffService.UpdateRoleAsync(id, result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return Problem(
            detail: ex.Message,
            statusCode: 403,
            title: "Forbidden"
        );
            }

            return Ok(new { message = "Employee permission has been modified successfully." });
        }

        /// <summary>
        /// Deletes a staff member by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the staff member to be deleted</param>
        /// <returns>A success message if the staff member is deleted, or a not found message if the staff is not found.</returns>
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

            try
            {
                await _staffService.DeleteStaffAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return Problem(
            detail: ex.Message,
        statusCode: 403,
            title: "Forbidden"
        );
            }
            return Ok(new { message = "The employee has been successfully deleted." });
        }
    }
}