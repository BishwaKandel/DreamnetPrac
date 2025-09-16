using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMSwebAPI.Models
{
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; } 
        public User user { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
