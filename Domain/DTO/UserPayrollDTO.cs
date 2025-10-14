using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class UserPayrollDTO
    {
        public string userId { get; set; }
        public string Name { get; set; }
        public List<PayrollDetailsDTO> payrolls { get; set; }
    }
}
