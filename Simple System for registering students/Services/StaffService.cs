using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Repositories;
using Simple_System_for_registering_students.Repositories.Interface;
using Simple_System_for_registering_students.Services.Interface;
using System.Security.Claims;


namespace Simple_System_for_registering_students.Services
{
    /// <summary>
    /// Service class responsible for handling business logic related to staff members.
    /// It provides functionality for authentication, registration, role management, 
    /// and interacting with staff data.
    /// </summary>
    public class StaffService : IStaffService
    {
    
    
        private  readonly IStaffRepository _staffRepository ;
        private readonly IHttpContextAccessor _httpContextAccessor;


        /// <summary>
        /// Initializes a new instance of the StaffService class.
        /// </summary>
        /// <param name="staffRepository">The repository used for staff-related operations.</param>
        /// <param name="httpContextAccessor">The accessor used to retrieve the HTTP context and user claims.</param>
        public StaffService(IStaffRepository staffRepository, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;

            _staffRepository = staffRepository;
        }

        /// <summary>
        /// Authenticates a staff member by email and password.
        /// </summary>
        /// <param name="email">The email of the staff member.</param>
        /// <param name="password">The password of the staff member.</param>
        /// <returns>The authenticated staff member if successful, otherwise null.</returns>
        public async Task<Staff> AuthenticateAsync(string email, string password)
        {
            var staff = await GetByEmailAsync(email);

            if (staff == null || !BCrypt.Net.BCrypt.Verify(password, staff.PasswordHash)) 
            {
                return null;
            }

            return staff;
        }

        /// <summary>
        /// Registers a new staff member if the email does not already exist.
        /// </summary>
        /// <param name="staff">The staff member to register.</param>
        /// <exception cref="InvalidOperationException">Thrown if a staff member with the given email already exists.</exception>
        public async Task RegisterStaffAsync(Staff staff)
        {
         
            var existingStaff = await GetByEmailAsync(staff.Email);
            if (existingStaff != null)
            {
                throw new InvalidOperationException("Employee with this email already exists.");
            }


           await AddStaffAsync(staff);

       

        }

        /// <summary>
        /// Retrieves a staff member by their email address.
        /// </summary>
        /// <param name="email">The email of the staff member.</param>
        /// <returns>The staff member with the given email, or null if not found.</returns>
        public async Task<Staff> GetByEmailAsync(string email)
        {
        

            var staff = await _staffRepository.GetStaffByEmailAsync(email);

            if (staff == null)
            {
                return null;
            }

            return staff;
        }

        /// <summary>
        /// Retrieves all staff members from the database, provided the user has sufficient permissions.
        /// </summary>
        /// <returns>A list of all staff members.</returns>
        /// <exception cref="Exception">Thrown if the user lacks the required permissions.</exception>
        public async Task<IEnumerable<Staff>> GetAllStaffsAsync()
        {
            if (!await _checkPermissionIsAdmin())
            {
                throw new Exception("You do not have sufficient permission.");
            }


            return await _staffRepository.GetAllStaffsAsync();
        }

        /// <summary>
        /// Retrieves a staff member by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the staff member.</param>
        /// <returns>The staff member with the given ID.</returns>
        public async Task<Staff> GetStaffByIdAsync(int id)
        {
       

            return await _staffRepository.GetStaffByIdAsync(id);
        }

        /// <summary>
        /// Checks if the current staff member is the first staff in the system.
        /// </summary>
        /// <returns>True if this is the first staff member, otherwise false.</returns>
        private async Task<bool> IsFirstStaffAsync()
        {
            return await _staffRepository.GetAllStaffsCountAsync() == 0;
        }

        /// <summary>
        /// Adds a new staff member to the system. If this is the first staff member, assigns them an admin role.
        /// </summary>
        /// <param name="staff">The staff member to add.</param>
        public async Task AddStaffAsync(Staff staff)
        {

     


            if (await IsFirstStaffAsync())
            {
               staff.Role = 6;
            }


            await _staffRepository.AddStaffAsync(staff);
        }


        /// <summary>
        /// Updates an existing staff member's details.
        /// </summary>
        /// <param name="staff">The staff member with updated details.</param>
        /// <exception cref="Exception">Thrown if the user lacks the required permissions.</exception>
        public async Task UpdateStaffAsync(Staff staff)
        {

            if (!await _checkPermissionIsAdmin())
            {
                throw new Exception("You do not have sufficient permission.");
            }


            await _staffRepository.UpdateStaffAsync(staff);
        }

        /// <summary>
        /// Deletes a staff member by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the staff member to delete.</param>
        /// <exception cref="Exception">Thrown if the user lacks the required permissions.</exception>
        public async Task DeleteStaffAsync(int id)
        {
            if (!await _checkPermissionIsAdmin())
            {
                throw new Exception("You do not have sufficient permission.");
            }


            await _staffRepository.DeleteStaffAsync(id);        
        }

        /// <summary>
        /// Updates the role of a staff member.
        /// </summary>
        /// <param name="staffId">The unique ID of the staff member whose role is to be updated.</param>
        /// <param name="newRole">The new role to assign to the staff member.</param>
        /// <exception cref="Exception">Thrown if the user lacks the required permissions.</exception>
        public async Task UpdateRoleAsync(int staffId, int newRole)
        {
            if (!await _checkPermissionIsAdmin())
            {
                throw new Exception("You do not have sufficient permission.");
            }

            await _staffRepository.UpdateRoleAsync(staffId, newRole);        
        }


        /// <summary>
        /// Retrieves the students assigned to a particular staff member.
        /// </summary>
        /// <param name="staffId">The unique ID of the staff member.</param>
        /// <returns>A list of students assigned to the specified staff member.</returns>
        /// <exception cref="Exception">Thrown if the user lacks the required permissions.</exception>
        public async Task<IEnumerable<Student>> GetStudentByStaffId(int staffId)
        {
           if(! await _checkPermissionIsAdmin())
            {
                throw new Exception("You do not have sufficient permission.");
            }

            return await _staffRepository.GetStudentByStaffId(staffId);
        }

        /// <summary>
        /// Checks if the current user has admin permissions.
        /// </summary>
        /// <returns>True if the current user is an admin, otherwise false.</returns>
        private async Task<bool> _checkPermissionIsAdmin()
        {
           
            var staff = await GetStaffByIdAsync(GetUserId());

            return (staff is not null && ((enPermissions)staff.Role & enPermissions.Admin) == enPermissions.Admin);
            
        }
        /// <summary>
        /// Retrieves the ID of the currently authenticated user.
        /// </summary>
        /// <returns>The user ID if authenticated, otherwise -1.</returns>
        public int GetUserId()
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

            if(int.TryParse(userIdClaim.Value,out int id))
            {
                return id;
            }

            return -1;
        }
    }
}
