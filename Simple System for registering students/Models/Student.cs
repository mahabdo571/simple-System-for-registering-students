using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Simple_System_for_registering_students.Models
{   
    /// <summary>
     /// Represents a student in the system.
     /// </summary>
    public class Student
    {
        /// <summary>
        /// The unique identifier for the student.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The first name of the student.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the student.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// The date of birth of the student.
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// The email address of the student.
        /// </summary>
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the student.
        /// </summary>
        [Phone]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The gender of the student (male or female).
        /// </summary>
        [Required]
        public bool Gender { get; set; } //true=Male / false =Fmale

        /// <summary>
        /// The address of the student.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The ID of the staff member who added the student.
        /// </summary>
        [Required]
        public int StaffId { get; set; }

        /// <summary>
        /// The staff member who added the student.
        /// </summary>
        [ForeignKey(nameof(StaffId))]
        [JsonIgnore]
        public Staff Staff { get; set; }

        /// <summary>
        /// The date and time when the student was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// The date and time when the student was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
