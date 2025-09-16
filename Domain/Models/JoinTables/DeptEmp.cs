//using Domain.Models.Base;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Models.JoinTables
//{
//    public class DeptEmp : EntityBase
//    {

//        [ForeignKey("Department")]
//        public Guid DepartmentId { get; set; }
//        public Department Department { get; set; }

//        [ForeignKey("User")]
//        public string Id { get; set; }
//        public User User { get; set; }
//        public DateTime DateOfJoining { get; set; } = DateTime.Now;
//    }
//}
