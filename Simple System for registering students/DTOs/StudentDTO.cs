using Simple_System_for_registering_students.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_System_for_registering_students.DTOs
{

        /// <summary>
        /// Represents the data transfer object for a student.
        /// </summary>
        public class StudentDto
    {
            /// <summary>
            /// The unique identifier for the student.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// The first name of the student.
            /// </summary>
            public string FirstName { get; set; }

            /// <summary>
            /// The last name of the student.
            /// </summary>
            public string LastName { get; set; }

            /// <summary>
            /// The date of birth of the student.
            /// </summary>
            public DateTime DateOfBirth { get; set; }

            /// <summary>
            /// The email address of the student.
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// The phone number of the student.
            /// </summary>
            public string PhoneNumber { get; set; }

            /// <summary>
            /// The gender of the student.
            /// </summary>
            public bool Gender { get; set; }

            /// <summary>
            /// The address of the student.
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// The ID of the staff member who created the student.
            /// </summary>
            public int StaffId { get; set; }



    }
}
