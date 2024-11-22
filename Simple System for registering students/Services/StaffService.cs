﻿using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Repositories;
using Simple_System_for_registering_students.Repositories.Interface;
using Simple_System_for_registering_students.Services.Interface;


namespace Simple_System_for_registering_students.Services
{
    public class StaffService : IStaffService
    {
    
    
        private  readonly IStaffRepository _staffRepository ;



        public StaffService(IStaffRepository staffRepository)
        {
       
         
            _staffRepository = staffRepository;
        }


        public async Task<Staff> AuthenticateAsync(string email, string password)
        {
            var staff = await GetByEmailAsync(email);

            if (staff == null || !BCrypt.Net.BCrypt.Verify(password, staff.PasswordHash)) 
            {
                return null;
            }

            return staff;
        }

        public async Task RegisterStaffAsync(Staff staff)
        {
         
            var existingStaff = await GetByEmailAsync(staff.Email);
            if (existingStaff != null)
            {
                throw new InvalidOperationException("Employee with this email already exists.");
            }


           await AddStaffAsync(staff);

       

        }

        public async Task<Staff> GetByEmailAsync(string email)
        {
            var staff = await _staffRepository.GetStaffByEmailAsync(email);

            if (staff == null)
            {
                return null;
            }

            return staff;
        }

        public async Task<IEnumerable<Staff>> GetAllStaffsAsync()
        {
            return await _staffRepository.GetAllStaffsAsync();
        }

        public async Task<Staff> GetStaffByIdAsync(int id)
        {
            return await _staffRepository.GetStaffByIdAsync(id);
        }

        private async Task<bool> IsFirstStaffAsync()
        {
            return await _staffRepository.GetAllStaffsCountAsync() == 0;
        }
 

        public async Task AddStaffAsync(Staff staff)
        {
            if(await IsFirstStaffAsync())
            {
               staff.Role = 1;
            }
           
            await _staffRepository.AddStaffAsync(staff);
            
         
        }



 

        public async Task UpdateStaffAsync(Staff staff)
        {
           await _staffRepository.UpdateStaffAsync(staff);
        }

        public async Task DeleteStaffAsync(int id)
        {
            await _staffRepository.DeleteStaffAsync(id);        
        }  
        
        public async Task UpdateRoleAsync(int staffId, int newRole)
        { 

            await _staffRepository.UpdateRoleAsync(staffId, newRole);        
        }

        public Task<IEnumerable<Student>> GetStudentByStaffId(int staffId)
        {
            return _staffRepository.GetStudentByStaffId(staffId);
        }
    }
}
