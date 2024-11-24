using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Services.Interface;

namespace Simple_System_for_registering_students.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
       
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _service.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentDto studentDTO)
        {
            var student = await _service.AddStudentAsync(studentDTO);
            return CreatedAtAction(nameof(GetAllStudents), new { id = student.Id }, student);
        }
    }
}