namespace Simple_System_for_registering_students.DTOs
{
    /// <summary>
    /// Represents the data transfer object for a staff member.
    /// </summary>
    public class StaffDto
    {
        /// <summary>
        /// The unique identifier for the staff member.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The username of the staff member.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The email address of the staff member.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The role of the staff member.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// The collection of students managed by the staff member.
        /// </summary>
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
    }
}
