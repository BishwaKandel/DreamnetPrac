//using Domain.Models.Base;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Models
//{
//    public class Payroll : EntityBase
//    {
//        [ForeignKey("User")]
//        public Guid Id { get; set; }
//        public User User { get; set; } = new User();
//        public int Year { get; set; }
//        public int Month { get; set; }
//        public decimal BasicSalary { get; set; }
//        public decimal Allowances { get; set; }
//        public decimal Deductions { get; set; }
//        public decimal NetSalary { get; set; }
//    }
//}
