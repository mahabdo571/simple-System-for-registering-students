using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Repositories.Interface;

namespace Simple_System_for_registering_students.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _context;

        public StaffRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Staff?> GetStaffByEmailAsync(string email)
        {
            var Staff = await _context.Staffs.FirstOrDefaultAsync(s => s.Email == email);

          return  Staff is not null ? Staff : null;

         
        }

    

        public async Task<IEnumerable<Staff>> GetAllStaffsAsync()
        {
            return await _context.Staffs.ToListAsync();
        }

        public async Task<int> GetAllStaffsCountAsync()
        {
            return await _context.Staffs.CountAsync();
        }

        public async Task<Staff> GetStaffByIdAsync(int id)
        {
            return await _context.Staffs.FindAsync(id);
        }

        public async Task AddStaffAsync(Staff Staff)
        {
            _context.Staffs.Add(Staff);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStaffAsync(Staff Staff)
        {
            _context.Staffs.Update(Staff);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStaffAsync(int id)
        {
            var Staff = await GetStaffByIdAsync(id);
            if (Staff != null)
            {
                _context.Staffs.Remove(Staff);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateRoleAsync(int staffId, int newRole)
        {
            var staff = await _context.Staffs.FindAsync(staffId);
            if (staff != null)
            {
                staff.Role = newRole; 
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<IEnumerable<Student>> GetStudentByStaffId(int staffId)
        {
            return await _context.Students.Where((st)=> st.StaffId == staffId).ToListAsync();
        }
    }
}
