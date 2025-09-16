using HRMSwebAPI.Models;

namespace HRMSwebAPI.DTO
{
    public class UserProfileDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> UserRoles { get; set; } 
    }
}