using System.ComponentModel.DataAnnotations;

namespace Simple_System_for_registering_students.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters")]
        public string Password { get; set; }
    }
}
