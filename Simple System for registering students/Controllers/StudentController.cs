using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Services;
using Simple_System_for_registering_students.Services.Interface;

namespace Simple_System_for_registering_students.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
       
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;
        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllStudents()
        {
            var staffList = await _studentService.GetAllStudentsAsync();

            if (staffList?.Count() == 0)
            {
                _logger.LogError("There are no Students in the system.");
                return NotFound(new { message = "There are no Students in the system." });
            }

            return Ok(staffList);
        }


        [HttpGet("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var staff = await _studentService.GetStudentByIdAsync(id);

            if (staff is null)
            {
                _logger.LogError("Employee is not present");
                return NotFound(new { message = "Employee is not present" });
            }

            return Ok(staff);
        }



        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddStudent([FromBody] StudentDto studentDTO)
        {
            var student = await _studentService.AddStudentAsync(studentDTO);


            return CreatedAtAction(nameof(GetAllStudents), new { id = student?.Id }, student);
        }
    }
}