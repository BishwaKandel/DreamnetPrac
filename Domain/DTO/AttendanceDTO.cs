using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AttendanceDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly? CheckInTime { get; set; }
        public TimeOnly? CheckOutTime { get; set; }
        public TimeSpan? TotalHoursWorked { get; set; }
        public AttendanceStatus status { get; set; }
    }
}
