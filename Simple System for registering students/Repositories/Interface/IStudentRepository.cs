using Simple_System_for_registering_students.Models;

namespace Simple_System_for_registering_students.Repositories.Interface
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
        Task<Staff> GetStaffWithStudents(int staffId);
    }
}
