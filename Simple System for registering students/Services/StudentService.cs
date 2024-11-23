using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Repositories;
using Simple_System_for_registering_students.Services.Interface;

namespace Simple_System_for_registering_students.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> AddStudentAsync(StudentDto studentDTO)
        {
            var student = new Student
            {
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                Email = studentDTO.Email
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }
    }
}
