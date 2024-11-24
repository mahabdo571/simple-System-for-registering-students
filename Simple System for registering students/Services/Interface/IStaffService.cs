using Simple_System_for_registering_students.Models;

namespace Simple_System_for_registering_students.Services.Interface
{
    public interface IStaffService
    {
        Task<Staff> AuthenticateAsync(string email, string password);
        Task<Staff> GetByEmailAsync(string email);
        Task<IEnumerable<Staff>> GetAllStaffsAsync();
        Task<IEnumerable<Student>> GetStudentByStaffId(int staffId);
        Task<Staff> GetStaffByIdAsync(int id);
        Task AddStaffAsync(Staff staff);
        Task RegisterStaffAsync(Staff staff);
        Task UpdateStaffAsync(Staff staff);
        Task DeleteStaffAsync(int id);
        Task UpdateRoleAsync(int staffId, int newRole);

    }
}
