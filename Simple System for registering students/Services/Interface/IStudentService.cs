using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;

namespace Simple_System_for_registering_students.Services.Interface
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student> AddStudentAsync(StudentDto studentDTO);
    }
}
