using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class RejectLeaveRequestDTO
    {
        public Guid LeaveRequestId { get; set; }
        public string RejectionReason { get; set; } = string.Empty;
    }
}
