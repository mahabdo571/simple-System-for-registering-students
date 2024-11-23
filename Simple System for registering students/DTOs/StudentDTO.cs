using System.ComponentModel.DataAnnotations;

namespace Simple_System_for_registering_students.DTOs
{
    public class StudentDTO
    {
        [Required]
        public string Name { get; set; }

        [Range(1, 100)]
        public int Age { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
