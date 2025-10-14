using Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSmvc.ViewModel
{
    public class LeaveRequestViewModel
    {
        public Guid? Id { get; set; }
        public DateOnly AppliedOn { get; set; }
        public DateOnly StartDate { get; set; } 
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Description { get; set; }

        [ValidateNever]
        [BindNever]
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
    }
}
