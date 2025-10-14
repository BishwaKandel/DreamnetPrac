using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class LeaveDetailsDTO
    {
        public Guid? Id { get; set; }
        public string RequestedById { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateOnly AppliedOn { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Description { get; set; }
    }
}
