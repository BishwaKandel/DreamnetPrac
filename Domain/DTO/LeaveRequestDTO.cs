using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class LeaveRequestDTO
    {
        public Guid Id { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string LeaveType { get; set; }
    }
}
