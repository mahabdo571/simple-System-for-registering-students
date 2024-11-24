using System.ComponentModel.DataAnnotations;

namespace Simple_System_for_registering_students.Models
{
    /// <summary>
    /// Represents a staff member in the system.
    /// </summary>
    public class Staff
    {
        /// <summary>
        /// The unique identifier for the staff member.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The username used for logging in.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        /// <summary>
        /// The hashed password for the staff member.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// The email address of the staff member.
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        /// <summary>
        /// The role of the staff member (e.g., admin or staff).
        /// </summary>
        [Required]
        public int Role { get; set; }

        /// <summary>
        /// The date and time when the staff member was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// The date and time when the staff member was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// A collection of students added by the staff member.
        /// </summary>
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
