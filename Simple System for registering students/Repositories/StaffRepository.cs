using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Repositories.Interface;

namespace Simple_System_for_registering_students.Repositories
{
    /// <summary>
    /// Repository class responsible for handling staff-related database operations.
    /// Implements IStaffRepository interface and provides methods for adding, updating, deleting, 
    /// and fetching staff data.
    /// </summary>
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor to initialize the StaffRepository with the database context.
        /// </summary>
        /// <param name="context">The AppDbContext instance used for database operations.</param>
        public StaffRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a staff member by their email address.
        /// </summary>
        /// <param name="email">The email address of the staff to search for.</param>
        /// <returns>A staff member if found, otherwise null.</returns>
        public async Task<Staff?> GetStaffByEmailAsync(string email)
        {
            var Staff = await _context.Staffs.FirstOrDefaultAsync(s => s.Email == email);

          return  Staff is not null ? Staff : null;

         
        }

        /// <summary>
        /// Retrieves all staff members from the database.
        /// </summary>
        /// <returns>A list of all staff members.</returns>
        public async Task<IEnumerable<Staff>> GetAllStaffsAsync()
        {
            return await _context.Staffs.ToListAsync();
        }

        /// <summary>
        /// Retrieves the total count of staff members in the database.
        /// </summary>
        /// <returns>The count of staff members.</returns>
        public async Task<int> GetAllStaffsCountAsync()
        {
            return await _context.Staffs.CountAsync();
        } 

        /// <summary>
        /// Retrieves a staff member by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the staff to retrieve.</param>
        /// <returns>A staff member if found, otherwise null.</returns>
        public async Task<Staff> GetStaffByIdAsync(int id)
        {
            return await _context.Staffs.FindAsync(id);
        }

        /// <summary>
        /// Adds a new staff member to the database.
        /// </summary>
        /// <param name="staff">The staff member to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddStaffAsync(Staff Staff)
        {
            _context.Staffs.Add(Staff);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing staff member's details in the database.
        /// </summary>
        /// <param name="staff">The staff member with updated details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateStaffAsync(Staff Staff)
        {
            _context.Staffs.Update(Staff);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a staff member from the database by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the staff member to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteStaffAsync(int id)
        {
            var Staff = await GetStaffByIdAsync(id);
            if (Staff != null)
            {
                _context.Staffs.Remove(Staff);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates the role of a staff member by their unique ID.
        /// </summary>
        /// <param name="staffId">The unique ID of the staff to update.</param>
        /// <param name="newRole">The new role to assign to the staff member.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateRoleAsync(int staffId, int newRole)
        {
            var staff = await _context.Staffs.FindAsync(staffId);
            if (staff != null)
            {
                staff.Role = newRole; 
                await _context.SaveChangesAsync(); 
            }
        }

        /// <summary>
        /// Retrieves a list of students associated with a particular staff member.
        /// </summary>
        /// <param name="staffId">The unique ID of the staff member to get students for.</param>
        /// <returns>A list of students associated with the given staff member.</returns>
        public async Task<IEnumerable<Student>> GetStudentByStaffId(int staffId)
        {
            return await _context.Students.Where((st)=> st.StaffId == staffId).ToListAsync();
        }
    }
}
