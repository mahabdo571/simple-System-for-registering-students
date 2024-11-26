using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Repositories;
using Simple_System_for_registering_students.Repositories.Interface;
using Simple_System_for_registering_students.Services.Interface;
using System.Security.Claims;

namespace Simple_System_for_registering_students.Services
{
    /// <summary>
    /// Service class responsible for handling business logic related to students.
    /// It interacts with the repository layer to manage student data, permissions, 
    /// and the associated staff.
    /// </summary>
    public class StudentService : IStudentService
    {

        private readonly IStudentRepository _studentRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the StudentService class.
        /// </summary>
        /// <param name="studentRepository">The repository used for student-related operations.</param>
        /// <param name="staffRepository">The repository used for staff-related operations.</param>
        /// <param name="httpContextAccessor">The accessor used to retrieve the HTTP context and user claims.</param>
        public StudentService(IStudentRepository studentRepository, IStaffRepository staffRepository, IHttpContextAccessor httpContextAccessor)
        {
            _studentRepository = studentRepository;
            _staffRepository = staffRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves all students from the database, provided the user has sufficient permissions.
        /// </summary>
        /// <returns>A list of students if the user has permission; otherwise, throws an exception.</returns>
        /// <exception cref="Exception">Thrown if the user lacks the required permissions.</exception>
        public async Task<IEnumerable<Student>?> GetAllStudentsAsync()
        {
            if (!await _checkPermissionIsAdminOrManger())
            {

                throw new Exception("You do not have sufficient permission.");
            }

            var students = await _studentRepository.GetAllStudentsAsync();

            return students is not null ? students : null;
        }

        /// <summary>
        /// Retrieves the staff member associated with a specific student.
        /// </summary>
        /// <param name="staffId">The unique ID of the staff member.</param>
        /// <returns>The staff member associated with the given ID.</returns>
        public async Task<Staff> GetStaffWithStudents(int staffId)
        {
            if (!await _checkPermissionIsAdminOrManger())
            {

                throw new Exception("You do not have sufficient permission.");
            }
            var staff = await _studentRepository.GetStaffWithStudents(staffId);

            return staff;
        }

        /// <summary>
        /// Adds a new student to the database.
        /// </summary>
        /// <param name="studentDTO">The data transfer object containing the student's details.</param>
        /// <returns>The created student.</returns>
        /// <exception cref="Exception">Thrown if the user lacks the required permissions.</exception>
        public async Task<Student?> AddStudentAsync(StudentDto studentDTO)
        {
            if (!await _checkPermissionIsAdminOrManger())
            {

                throw new Exception("You do not have sufficient permission.");
            }
            var student = new Student
            {
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                Email = studentDTO.Email,
                Address = studentDTO.Address,
                DateOfBirth = studentDTO.DateOfBirth,
                Gender = studentDTO.Gender,
                PhoneNumber = studentDTO.PhoneNumber,
                StaffId = GetStaffId(), 
                Staff = await GetStaffWithStudents(GetStaffId()),


            };



            await _studentRepository.AddStudentAsync(student);


            return student;
        }

        /// <summary>
        /// Retrieves a student by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the student.</param>
        /// <returns>The student if found; otherwise, null.</returns>
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            //genral
            var student = await _studentRepository.GetStudentByIdAsync(id);

            return student is not null ? student : null;
        }

        /// <summary>
        /// Updates the details of an existing student.
        /// </summary>
        /// <param name="student">The student with updated details.</param>
        /// <exception cref="Exception">Thrown if the user lacks the required permissions.</exception>
        public async Task UpdateStudentAsync(Student student)
        {


            if (!await _checkPermissionIsAdminOrManger())
            {

                throw new Exception("You do not have sufficient permission.");
            }
            student.StaffId = GetStaffId();
            await _studentRepository.UpdateStudentAsync(student);
        }

        /// <summary>
        /// Deletes a student by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the student to be deleted.</param>
        /// <exception cref="Exception">Thrown if the user lacks the required permissions.</exception>
        public async Task DeleteStudentAsync(int id)
        {
            if (!await _checkPermissionIsAdminOrManger())
            {
                throw new Exception("You do not have sufficient permission.");
            }
            await _studentRepository.DeleteStudentAsync(id);
        }

        /// <summary>
        /// Checks if the current user has the necessary permissions (Admin or Manager).
        /// </summary>
        /// <returns>True if the user has the required permissions; otherwise, false.</returns>
        private async Task<bool> _checkPermissionIsAdminOrManger()
        {

            var staff = await _staffRepository.GetStaffByIdAsync(GetStaffId());

            return (staff is not null && ((enPermissions)staff.Role & enPermissions.Manger) == enPermissions.Manger);

        }

        /// <summary>
        /// Retrieves the ID of the currently authenticated user.
        /// </summary>
        /// <returns>The user ID if authenticated; otherwise, -1.</returns>
        public int GetStaffId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return -1;
            }


            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return -1;
            }

            if (int.TryParse(userIdClaim.Value, out int id))
            {
                return id;
            }

            return -1;
        }

    }
}
