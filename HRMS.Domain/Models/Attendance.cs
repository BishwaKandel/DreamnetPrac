using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Models
{
    public class Attendance
    {
        public Guid Id { get; set; }

        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public TimeSpan TotalHoursWorked { get; set; }
        public string status { get; set; } // e.g., Present, Absent, Leave, etc.

    }
}
