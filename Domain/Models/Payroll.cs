using Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Payroll : EntityBase
    {
        [ForeignKey("User")]
        [Required(ErrorMessage = "UserId is required.")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Range(2000, 9999, ErrorMessage = "Year must be a valid year between 2000 and 9999.")]
        public int? Year { get; set; }

        [Range(1, 12, ErrorMessage = "Month must be a valid month between 1 and 12.")]
        public int? Month { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Basic Salary cannot be negative.")]
        public decimal BasicSalary { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Allowances cannot be negative.")]
        public decimal Allowances { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Deductions cannot be negative.")]
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }

        public bool ValidateNetSalary()
        {
            return NetSalary == BasicSalary + Allowances - Deductions;
        }
    }
}
