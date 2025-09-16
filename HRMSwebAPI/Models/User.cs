using System;

namespace HRMSwebAPI.Models
{
    public class User
    {
        public Guid  Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DOB { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        //public ICollection<UserRole> UserRoles { get; set; } // navigation to UserRole
    }
}
