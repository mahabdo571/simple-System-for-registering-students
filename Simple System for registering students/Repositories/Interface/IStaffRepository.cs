using Simple_System_for_registering_students.Models;

namespace Simple_System_for_registering_students.Repositories.Interface
{
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetAllStaffsAsync();
        Task<Staff> GetStaffByIdAsync(int id);
        Task<Staff> GetStaffByEmailAsync(string email);
        Task AddStaffAsync(Staff staff);
        Task UpdateStaffAsync(Staff staff);
        Task DeleteStaffAsync(int id);
    }
}
