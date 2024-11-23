using Simple_System_for_registering_students.Models;

namespace Simple_System_for_registering_students.Services.Interface
{
    public interface IAuthService
    {
        Task<Staff> AuthenticateAsync(string email, string password);
        string GenerateJwtToken(Staff staff);
    }
}
