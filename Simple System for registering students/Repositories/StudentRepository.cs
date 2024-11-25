using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Repositories.Interface;

namespace Simple_System_for_registering_students.Repositories
{
    /// <summary>
    /// Repository class responsible for handling student-related database operations.
    /// Implements IStudentRepository interface and provides methods for adding, updating, deleting, 
    /// and fetching student data.
    /// </summary>
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor to initialize the StudentRepository with the database context.
        /// </summary>
        /// <param name="context">The AppDbContext instance used for database operations.</param>
        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all students from the database.
        /// </summary>
        /// <returns>A list of all students.</returns>
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        /// <summary>
        /// Retrieves a student by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the student to retrieve.</param>
        /// <returns>A student if found, otherwise null.</returns>
        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        /// <summary>
        /// Adds a new student to the database.
        /// </summary>
        /// <param name="student">The student to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
         
        }

        /// <summary>
        /// Updates an existing student's details in the database.
        /// </summary>
        /// <param name="student">The student with updated details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a student from the database by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the student to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves the staff member associated with a specific student.
        /// </summary>
        /// <param name="staffId">The unique ID of the staff member to retrieve.</param>
        /// <returns>The staff member associated with the given ID, or null if not found.</returns>
        public async Task<Staff> GetStaffWithStudents(int staffId)
        {
            var staff =await _context.Staffs.FirstOrDefaultAsync<Staff>(s => s.Id == staffId);

            return staff;
        }
    }
}
