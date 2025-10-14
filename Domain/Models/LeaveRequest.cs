using Domain.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class LeaveRequest : EntityBase
    {
        public string RequestedById { get; set; }

        [ForeignKey("RequestedById")]
        public User RequestedBy { get; set; }  // Should have Role = "User"
        public DateOnly AppliedOn { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }

        public string  Description { get; set; }
        public string?  Remarks { get; set; }
        public LeaveStatus Status { get; set; } 
        public LeaveType LeaveType { get; set; } 
    }
    
    public enum LeaveStatus
    {
        Pending, 
        Approved,
        Rejected
    }

    public enum LeaveType
    {
        Sick,
        Casual,
        Unpaid
    }
}
