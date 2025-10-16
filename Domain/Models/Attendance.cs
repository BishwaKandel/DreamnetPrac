using Domain.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Attendance : EntityBase
    {
        [Required(ErrorMessage = "UserId is required.")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Check-in time is required.")]
        public TimeOnly CheckInTime { get; set; }

        public TimeOnly? CheckOutTime { get; set; }

        public TimeSpan? TotalHoursWorked { get; set; }

        [EnumDataType(typeof(AttendanceStatus))]
        public AttendanceStatus Status { get; set; }
    }

    public enum AttendanceStatus
    {
        Present,
        Late,
        OnLeave
    }
}
