using System.ComponentModel.DataAnnotations;

namespace Simple_System_for_registering_students.DTOs
{
    public class EditStaffDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters")]
        public string Password { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Required]
        public int Role { get; set; }

    }
}
