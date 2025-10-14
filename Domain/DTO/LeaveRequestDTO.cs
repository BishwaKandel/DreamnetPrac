using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Domain.DTO
{
    public class LeaveRequestDTO
    {
        public Guid? Id { get; set; }
        public string RequestedById { get; set; }
        public DateOnly AppliedOn { get; set; } 
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Description { get; set; }

    }
}
