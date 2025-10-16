using Domain.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class LeaveRequest : EntityBase
    {
        [Required(ErrorMessage = "RequestedById is required.")]
        public string RequestedById { get; set; }

        [ForeignKey("RequestedById")]
        public User RequestedBy { get; set; }  // Should have Role = "User"

        [Required(ErrorMessage = "AppliedOn date is required.")]
        public DateOnly AppliedOn { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Reason is required.")]
        public string Reason { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public string? Remarks { get; set; }

        [EnumDataType(typeof(LeaveStatus))]
        public LeaveStatus Status { get; set; }

        [EnumDataType(typeof(LeaveType))]
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
