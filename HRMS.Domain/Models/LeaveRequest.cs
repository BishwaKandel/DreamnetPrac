using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Models
{
    public class LeaveRequest
    {
        public Guid Id { get; set; }

        [ForeignKey("Employee")]
        public Guid EId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } 
        public string LeaveType { get; set; }

    }
}
