namespace HRMSwebAPI.DTO
{
    public class UserDto
    {
        //public string Name { get; set; } = null!;
        public string Email { get; set; }= null!;
        public string Password { get; set; } = null!;
        public string role { get; set; } = "User"; // Default role
    }
}
