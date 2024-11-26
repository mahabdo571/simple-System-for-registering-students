using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Services;
using Simple_System_for_registering_students.Services.Interface;

namespace Simple_System_for_registering_students.Controllers
{
    /// <summary>
    /// Controller responsible for handling student operations such as registration, updating, retrieving, and deletion.
    /// All actions are protected by authorization unless explicitly marked as AllowAnonymous.
    /// </summary>
    [Authorize]
    [Route("api/Student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
       
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        /// <summary>
        /// Constructor that initializes the StudentController with necessary services.
        /// </summary>
        /// <param name="studentService">Service for interacting with student data.</param>
        /// <param name="logger">Logger for logging messages and errors.</param>
        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all students from the system.
        /// </summary>
        /// <returns>A list of students.</returns>
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllStudents()
        {
            try { 
            var staffList = await _studentService.GetAllStudentsAsync();

            if (staffList?.Count() == 0)
            {
                _logger.LogError("There are no Students in the system.");
                return NotFound(new { message = "There are no Students in the system." });
            }

            return Ok(staffList);
        }catch(Exception ex)
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
        /// Gets details of a student by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the student to retrieve.</param>
        /// <returns>The student's details.</returns>
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try { 
            var staff = await _studentService.GetStudentByIdAsync(id);

            if (staff is null)
            {
                _logger.LogError("Employee is not present");
                return NotFound(new { message = "Employee is not present" });
            }

            return Ok(staff);
        }catch(Exception ex)
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
        /// Adds a new student to the system.
        /// </summary>
        /// <param name="studentDTO">The student data to be added.</param>
        /// <returns>The created student data along with its ID.</returns>
        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddStudent([FromBody] StudentDto studentDTO)
        {

           
            try
            {
                var student = await _studentService.AddStudentAsync(studentDTO);
                return CreatedAtAction(nameof(GetAllStudents), new { id = student?.Id }, student);

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
        /// Updates an existing student's details by their ID.
        /// </summary>
        /// <param name="id">The ID of the student to update.</param>
        /// <param name="studentDTO">The updated student data.</param>
        /// <returns>A message indicating the result of the update operation.</returns>
        [HttpPut("{id}", Name = "StudentUpdateById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> StudentUpdateById(int id, [FromBody] StudentDto studentDTO)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
            {
                _logger.LogError("student is not present");
                return NotFound(new { message = "student is not present" });
            }


            student.FirstName = studentDTO.FirstName;
            student.LastName = studentDTO.LastName;
            student.PhoneNumber = studentDTO.PhoneNumber;
         
            student.DateOfBirth = studentDTO.DateOfBirth;
            student.Address = studentDTO.Address;
            student.Gender = studentDTO.Gender;
            student.Email = studentDTO.Email;
            student.UpdatedAt = DateTime.Now;
            try
            {
                await _studentService.UpdateStudentAsync(student);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return Problem(
            detail: ex.Message,
            statusCode: 403,
            title: "Forbidden"
        ); 
            }
           
            return Ok(new { message = "The student has been modified successfully." });
        }

        /// <summary>
        /// Deletes a student by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the student to delete.</param>
        /// <returns>A message indicating the result of the deletion operation.</returns>
        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student is null)
            {
                _logger.LogWarning("student is not present");
                return NotFound(new { message = "student is not present" });
            }

            try
            {
                await _studentService.DeleteStudentAsync(id);
            }catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return Problem(
            detail: ex.Message,
            statusCode: 403,
            title: "Forbidden"
        );
            }
            return Ok(new { message = "The student has been successfully deleted." });
        }

    }
}