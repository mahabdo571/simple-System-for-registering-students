using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;

namespace Simple_System_for_registering_students.Services.Interface
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>?> GetAllStudentsAsync();
        Task<Student?> AddStudentAsync(StudentDto studentDTO);
        Task<Student?> GetStudentByIdAsync(int id);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
    }
}
