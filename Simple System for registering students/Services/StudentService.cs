using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Data;
using Simple_System_for_registering_students.DTOs;
using Simple_System_for_registering_students.Models;
using Simple_System_for_registering_students.Repositories;
using Simple_System_for_registering_students.Repositories.Interface;
using Simple_System_for_registering_students.Services.Interface;

namespace Simple_System_for_registering_students.Services
{
    public class StudentService : IStudentService
    {

        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Student>?>  GetAllStudentsAsync()
        {
          

            var students = await _studentRepository.GetAllStudentsAsync();

            return students is not null ? students : null;
        }

        public async Task<Student?> AddStudentAsync(StudentDto studentDTO)
        {
            var student = new Student
            {
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                Email = studentDTO.Email,
                Address = studentDTO.Address,
                DateOfBirth = studentDTO.DateOfBirth,
                Gender = studentDTO.Gender,
                PhoneNumber =studentDTO.PhoneNumber,
                StaffId = studentDTO.StaffId,
             


            };

           

          await  _studentRepository.AddStudentAsync(student);
        

            return student;
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
          var student = await _studentRepository.GetStudentByIdAsync(id);

            return student is not null ? student : null;
        }
    }
}
