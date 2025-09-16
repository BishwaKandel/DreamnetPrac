//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Models
//{
//    public class UserRole
//    {
//        [Key]
//        public Guid Id { get; set; }

//        [ForeignKey("User")]
//        public Guid UserId { get; set; }
//        public User user { get; set; }

//        [ForeignKey("Role")]
//        public Guid RoleId { get; set; }
//        public Role Role { get; set; }
//    }
//}
