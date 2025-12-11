namespace Clinic_System.Application.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; } // Id الدكتور (Primary Key)
        public string UserId { get; set; } = null!; // Id الهوية (Identity Key)
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
