using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Models
{
    public class Payroll
    {
        public Guid Id { get; set; }

        [ForeignKey("Employee")]
        public Guid EId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
    }
}
